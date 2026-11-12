using System;
using System.IO;
using CookieClicker.Models;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Sección del guardado para el dinero. El modelo no sabe que se guarda: este adaptador lo hace por él (SRP).
    /// </summary>
    public sealed class CurrencySaveHandler : ISaveable
    {
        private readonly IReadOnlyCurrency _source;
        private readonly ICurrencyRestorer _target;

        public CurrencySaveHandler(IReadOnlyCurrency source, ICurrencyRestorer target)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public string SaveKey => "currency";
        public int DataVersion => 1;

        public void Save(BinaryWriter writer)
        {
            writer.Write(_source.Balance);
            writer.Write(_source.TotalEarned);
        }

        public void Load(BinaryReader reader, int dataVersion)
        {
            switch (dataVersion)
            {
                case 1:
                    double balance = reader.ReadDouble();
                    double totalEarned = reader.ReadDouble();
                    _target.Restore(balance, totalEarned);
                    break;

                // Si en el futuro cambias lo que se guarda, sube DataVersion a 2,
                // añade "case 2" y deja "case 1" para seguir leyendo partidas antiguas.

                default:
                    throw new NotSupportedException($"Versión de datos de dinero no soportada: {dataVersion}.");
            }
        }
    }
}
