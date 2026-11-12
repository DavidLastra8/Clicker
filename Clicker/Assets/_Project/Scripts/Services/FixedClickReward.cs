using System;

namespace CookieClicker.Services
{
    /// <summary>Recompensa fija por clic.</summary>
    public sealed class FixedClickReward : IClickReward
    {
        private readonly double _amount;

        public FixedClickReward(double amount)
        {
            if (amount <= 0d)
                throw new ArgumentOutOfRangeException(nameof(amount), "La recompensa por clic debe ser positiva.");

            _amount = amount;
        }

        public double GetAmount() => _amount;
    }
}
