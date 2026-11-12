using System;
using System.IO;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Guarda los bytes en un archivo local. Escribe primero en un .tmp y luego lo sustituye,
    /// para no dejar un guardado corrupto si el juego se cierra a mitad de escritura.
    /// </summary>
    public sealed class FileSaveStorage : ISaveStorage
    {
        private readonly string _path;
        private readonly string _tempPath;

        public FileSaveStorage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("La ruta del guardado no puede estar vacía.", nameof(path));

            _path = path;
            _tempPath = path + ".tmp";
        }

        public bool Exists => File.Exists(_path);

        public byte[] Read() => File.ReadAllBytes(_path);

        public void Write(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(_tempPath, data);

            if (!File.Exists(_path))
            {
                File.Move(_tempPath, _path);
                return;
            }

            try
            {
                File.Replace(_tempPath, _path, null);
            }
            catch (PlatformNotSupportedException)
            {
                File.Copy(_tempPath, _path, overwrite: true);
                File.Delete(_tempPath);
            }
        }

        public void Delete()
        {
            if (File.Exists(_path))
                File.Delete(_path);
            if (File.Exists(_tempPath))
                File.Delete(_tempPath);
        }
    }
}
