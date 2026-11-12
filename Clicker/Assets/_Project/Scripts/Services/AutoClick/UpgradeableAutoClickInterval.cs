using System;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>
    /// Intervalo = base * (1 - reducción) ^ nivel, nunca por debajo del mínimo.
    /// Con base 2 s y reducción 0.1: 2 s, 1.8 s, 1.62 s...
    /// </summary>
    public sealed class UpgradeableAutoClickInterval : IAutoClickInterval
    {
        private readonly double _baseSeconds;
        private readonly double _minSeconds;
        private readonly double _reductionPerLevel;
        private readonly IReadOnlyUpgrade _speedUpgrade;

        public UpgradeableAutoClickInterval(double baseSeconds, double minSeconds, IReadOnlyUpgrade speedUpgrade, double reductionPerLevel)
        {
            if (baseSeconds <= 0d)
                throw new ArgumentOutOfRangeException(nameof(baseSeconds));
            if (minSeconds <= 0d || minSeconds > baseSeconds)
                throw new ArgumentOutOfRangeException(nameof(minSeconds), "El mínimo debe ser > 0 y <= que el intervalo base.");
            if (reductionPerLevel < 0d || reductionPerLevel >= 1d)
                throw new ArgumentOutOfRangeException(nameof(reductionPerLevel), "La reducción debe estar entre 0 y 1 (0.1 = 10 %).");

            _baseSeconds = baseSeconds;
            _minSeconds = minSeconds;
            _speedUpgrade = speedUpgrade ?? throw new ArgumentNullException(nameof(speedUpgrade));
            _reductionPerLevel = reductionPerLevel;
        }

        public double Seconds =>
            Math.Max(_minSeconds, _baseSeconds * Math.Pow(1d - _reductionPerLevel, _speedUpgrade.Level));
    }
}
