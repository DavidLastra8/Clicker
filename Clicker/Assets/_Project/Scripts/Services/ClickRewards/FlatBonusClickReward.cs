using System;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>
    /// Decorador: suma una cantidad fija por cada nivel de una mejora.
    /// Recompensa = base + nivel * bonusPerLevel.
    /// </summary>
    public sealed class FlatBonusClickReward : IClickReward
    {
        private readonly IClickReward _inner;
        private readonly IReadOnlyUpgrade _upgrade;
        private readonly double _bonusPerLevel;

        public FlatBonusClickReward(IClickReward inner, IReadOnlyUpgrade upgrade, double bonusPerLevel)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _upgrade = upgrade ?? throw new ArgumentNullException(nameof(upgrade));
            _bonusPerLevel = bonusPerLevel;
        }

        public double GetAmount() => _inner.GetAmount() + _upgrade.Level * _bonusPerLevel;
    }
}
