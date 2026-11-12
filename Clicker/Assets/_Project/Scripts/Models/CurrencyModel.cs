using System;

namespace CookieClicker.Models
{
    /// <summary>
    /// Modelo del dinero del jugador. C# puro: no conoce Unity ni la UI, así que se puede testear en EditMode.
    /// Se usa double porque en un incremental las cifras crecen muchísimo.
    /// </summary>
    public sealed class CurrencyModel : IReadOnlyCurrency, ICurrencyReceiver, ICurrencySpender
    {
        public double Balance { get; private set; }
        public double TotalEarned { get; private set; }

        public event Action<double> BalanceChanged;

        public CurrencyModel(double initialBalance = 0d)
        {
            if (initialBalance < 0d)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "El saldo inicial no puede ser negativo.");

            Balance = initialBalance;
        }

        public void Add(double amount)
        {
            if (amount <= 0d)
                throw new ArgumentOutOfRangeException(nameof(amount), "La cantidad a sumar debe ser positiva.");

            Balance += amount;
            TotalEarned += amount;
            BalanceChanged?.Invoke(Balance);
        }

        public bool TrySpend(double amount)
        {
            if (amount <= 0d)
                throw new ArgumentOutOfRangeException(nameof(amount), "La cantidad a gastar debe ser positiva.");

            if (Balance < amount)
                return false;

            Balance -= amount;
            BalanceChanged?.Invoke(Balance);
            return true;
        }
    }
}
