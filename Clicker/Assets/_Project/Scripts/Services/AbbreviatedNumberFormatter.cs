using System;
using System.Globalization;

namespace CookieClicker.Services
{
    /// <summary>
    /// Formatea con sufijos típicos de incrementales: 999, 1.5K, 2.34M, 7B...
    /// Más allá del último sufijo pasa a notación científica.
    /// Trunca en vez de redondear para no mostrar nunca más dinero del que hay.
    /// </summary>
    public sealed class AbbreviatedNumberFormatter : INumberFormatter
    {
        private static readonly string[] Suffixes =
        {
            "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
        };

        private readonly CultureInfo _culture;

        public AbbreviatedNumberFormatter(CultureInfo culture = null)
        {
            _culture = culture ?? CultureInfo.InvariantCulture;
        }

        public string Format(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                return "∞";

            if (value < 0d)
                return "-" + Format(-value);

            if (value < 1000d)
                return Math.Floor(value).ToString("0", _culture);

            int tier = (int)Math.Floor(Math.Log10(value) / 3d);

            if (tier >= Suffixes.Length)
                return value.ToString("0.##e0", _culture);

            double scaled = value / Math.Pow(1000d, tier);
            double truncated = Math.Floor(scaled * 100d) / 100d;

            return truncated.ToString("0.##", _culture) + Suffixes[tier];
        }
    }
}
