using System;
using CookieClicker.Core;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>
    /// Ayudantes: cada intervalo, cada ayudante hace un clic que da lo mismo que un clic del jugador
    /// (incluidas las mejoras de dinero por clic y multiplicador).
    /// </summary>
    public sealed class AutoClickerService : ITickable
    {
        private readonly IReadOnlyUpgrade _helperUpgrade;
        private readonly double _helpersPerLevel;
        private readonly IAutoClickInterval _interval;
        private readonly IClickReward _reward;
        private readonly ICurrencyReceiver _currency;

        private double _elapsed;

        public AutoClickerService(IReadOnlyUpgrade helperUpgrade, double helpersPerLevel,
            IAutoClickInterval interval, IClickReward reward, ICurrencyReceiver currency)
        {
            if (helpersPerLevel <= 0d)
                throw new ArgumentOutOfRangeException(nameof(helpersPerLevel));

            _helperUpgrade = helperUpgrade ?? throw new ArgumentNullException(nameof(helperUpgrade));
            _helpersPerLevel = helpersPerLevel;
            _interval = interval ?? throw new ArgumentNullException(nameof(interval));
            _reward = reward ?? throw new ArgumentNullException(nameof(reward));
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public double HelperCount => _helperUpgrade.Level * _helpersPerLevel;

        public void Tick(float deltaTime)
        {
            double helpers = HelperCount;
            if (helpers <= 0d)
            {
                _elapsed = 0d;
                return;
            }

            _elapsed += deltaTime;
            double interval = _interval.Seconds;
            if (_elapsed < interval)
                return;

            // Si el frame fue largo, se cobran todos los ciclos que han pasado.
            int cycles = (int)(_elapsed / interval);
            _elapsed -= cycles * interval;

            double amount = _reward.GetAmount() * helpers * cycles;
            if (amount > 0d)
                _currency.Add(amount);
        }
    }
}
