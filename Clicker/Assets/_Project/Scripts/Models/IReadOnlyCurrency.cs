using System;

namespace CookieClicker.Models
{
    /// <summary>
    /// Acceso de solo lectura al dinero. Lo usan quienes solo necesitan mostrar o consultar el saldo.
    /// </summary>
    public interface IReadOnlyCurrency
    {
        double Balance { get; }
        double TotalEarned { get; }

        /// <summary>Se lanza cada vez que cambia el saldo. Envía el nuevo saldo.</summary>
        event Action<double> BalanceChanged;
    }
}
