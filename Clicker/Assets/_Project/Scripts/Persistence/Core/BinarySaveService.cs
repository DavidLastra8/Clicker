using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Guardado binario por secciones.
    ///
    /// Formato del archivo:
    ///   [int  magic "CKSV"] [int formatVersion] [int sectionCount]
    ///   por cada sección: [string key] [int dataVersion] [int length] [byte[length] payload]
    ///
    /// Cada sección lleva su longitud, así que:
    ///  - Se pueden añadir secciones nuevas sin romper guardados antiguos (la sección nueva simplemente no está).
    ///  - Las secciones que ya no existen en el juego se saltan sin leerlas.
    ///  - Si una sección falla al cargar, las demás se cargan igualmente.
    /// </summary>
    public sealed class BinarySaveService : ISaveService
    {
        private const int Magic = 0x56534B43; // "CKSV" en little-endian
        private const int FormatVersion = 1;

        private readonly ISaveStorage _storage;
        private readonly Action<Exception> _onError;
        private readonly Dictionary<string, ISaveable> _saveables = new(StringComparer.Ordinal);

        public BinarySaveService(ISaveStorage storage, Action<Exception> onError = null)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _onError = onError;
        }

        public bool HasSave => _storage.Exists;

        public void Register(ISaveable saveable)
        {
            if (saveable == null)
                throw new ArgumentNullException(nameof(saveable));
            if (string.IsNullOrWhiteSpace(saveable.SaveKey))
                throw new ArgumentException("El SaveKey no puede estar vacío.", nameof(saveable));
            if (_saveables.ContainsKey(saveable.SaveKey))
                throw new InvalidOperationException($"Ya hay una sección registrada con la clave '{saveable.SaveKey}'.");

            _saveables.Add(saveable.SaveKey, saveable);
        }

        public void Unregister(ISaveable saveable)
        {
            if (saveable != null)
                _saveables.Remove(saveable.SaveKey);
        }

        public bool Save()
        {
            try
            {
                _storage.Write(Serialize());
                return true;
            }
            catch (Exception e)
            {
                _onError?.Invoke(e);
                return false;
            }
        }

        public bool Load()
        {
            if (!_storage.Exists)
                return false;

            Dictionary<string, Section> sections;
            try
            {
                // Fase 1: validar y leer la estructura completa antes de tocar el estado del juego.
                sections = Parse(_storage.Read());
            }
            catch (Exception e)
            {
                _onError?.Invoke(e);
                return false;
            }

            // Fase 2: repartir cada sección. Un fallo en una no impide cargar las demás.
            bool allLoaded = true;
            foreach (var pair in _saveables)
            {
                if (!sections.TryGetValue(pair.Key, out var section))
                    continue; // Sección nueva que aún no existía en este guardado: se queda con sus valores por defecto.

                try
                {
                    using var stream = new MemoryStream(section.Payload, writable: false);
                    using var reader = new BinaryReader(stream, Encoding.UTF8);
                    pair.Value.Load(reader, section.DataVersion);
                }
                catch (Exception e)
                {
                    allLoaded = false;
                    _onError?.Invoke(new InvalidDataException($"No se pudo cargar la sección '{pair.Key}'.", e));
                }
            }

            return allLoaded;
        }

        public void Delete() => _storage.Delete();

        private byte[] Serialize()
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(Magic);
                writer.Write(FormatVersion);
                writer.Write(_saveables.Count);

                foreach (var saveable in _saveables.Values)
                {
                    byte[] payload = SerializeSection(saveable);
                    writer.Write(saveable.SaveKey);
                    writer.Write(saveable.DataVersion);
                    writer.Write(payload.Length);
                    writer.Write(payload);
                }
            }
            return stream.ToArray();
        }

        private static byte[] SerializeSection(ISaveable saveable)
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                saveable.Save(writer);
            return stream.ToArray();
        }

        private static Dictionary<string, Section> Parse(byte[] data)
        {
            using var stream = new MemoryStream(data, writable: false);
            using var reader = new BinaryReader(stream, Encoding.UTF8);

            if (reader.ReadInt32() != Magic)
                throw new InvalidDataException("El archivo no es un guardado válido.");

            int formatVersion = reader.ReadInt32();
            if (formatVersion > FormatVersion)
                throw new InvalidDataException($"Guardado de una versión más nueva ({formatVersion}) que la soportada ({FormatVersion}).");

            int count = reader.ReadInt32();
            if (count < 0)
                throw new InvalidDataException("Número de secciones no válido.");

            var sections = new Dictionary<string, Section>(count, StringComparer.Ordinal);
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                int dataVersion = reader.ReadInt32();
                int length = reader.ReadInt32();

                if (length < 0 || length > stream.Length - stream.Position)
                    throw new InvalidDataException($"Longitud no válida en la sección '{key}'.");

                sections[key] = new Section(dataVersion, reader.ReadBytes(length));
            }
            return sections;
        }

        private readonly struct Section
        {
            public readonly int DataVersion;
            public readonly byte[] Payload;

            public Section(int dataVersion, byte[] payload)
            {
                DataVersion = dataVersion;
                Payload = payload;
            }
        }
    }
}
