using System;
using System.Collections.Generic;
using System.IO;
using CookieClicker.Models;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Sección del guardado con el nivel de todas las mejoras, identificadas por su Id.
    /// Las mejoras nuevas que no estén en el archivo se quedan a nivel 0; las que ya no existan se ignoran.
    /// </summary>
    public sealed class UpgradesSaveHandler : ISaveable
    {
        private readonly Dictionary<string, IRestorableUpgrade> _upgrades = new(StringComparer.Ordinal);

        public UpgradesSaveHandler(IEnumerable<IRestorableUpgrade> upgrades)
        {
            if (upgrades == null)
                throw new ArgumentNullException(nameof(upgrades));

            foreach (var upgrade in upgrades)
            {
                if (_upgrades.ContainsKey(upgrade.Id))
                    throw new ArgumentException($"Hay dos mejoras con el mismo id '{upgrade.Id}'.", nameof(upgrades));
                _upgrades.Add(upgrade.Id, upgrade);
            }
        }

        public string SaveKey => "upgrades";
        public int DataVersion => 1;

        public void Save(BinaryWriter writer)
        {
            writer.Write(_upgrades.Count);
            foreach (var upgrade in _upgrades.Values)
            {
                writer.Write(upgrade.Id);
                writer.Write(upgrade.Level);
            }
        }

        public void Load(BinaryReader reader, int dataVersion)
        {
            if (dataVersion != 1)
                throw new NotSupportedException($"Versión de datos de mejoras no soportada: {dataVersion}.");

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string id = reader.ReadString();
                int level = reader.ReadInt32();

                if (_upgrades.TryGetValue(id, out var upgrade))
                    upgrade.RestoreLevel(level);
            }
        }
    }
}
