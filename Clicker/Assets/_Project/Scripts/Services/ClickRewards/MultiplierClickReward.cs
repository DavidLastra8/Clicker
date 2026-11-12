using System;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>
    /// Decorador: multiplica la recompensa según el nivel de una mejora.
    /// Recompensa = base * (1 + nivel * multiplierPerLevel). Con 0.5 por nivel: x1, x1.5, x2, x2.5...
    /// </summary>
    public sealed class MultiplierClickReward : IClickReward
    {
        private readonly IClickReward _inner;
        private readonly IReadOnlyUpgrade _upgrade;
        private readonly double _multiplierPerLevel;

        public MultiplierClickReward(IClickReward inner, IReadOnlyUpgrade upgrade, double multiplierPerLevel)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _upgrade = upgrade ?? throw new ArgumentNullException(nameof(upgrade));
            _multiplierPerLevel = multiplierPerLevel;
        }

        public double GetAmount() => _inner.GetAmount() * (1d + _upgrade.Level * _multiplierPerLevel);
    }
}
