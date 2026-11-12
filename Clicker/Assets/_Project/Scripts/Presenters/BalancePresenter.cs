using System;
using CookieClicker.Models;
using CookieClicker.Services;
using CookieClicker.Views;

namespace CookieClicker.Presenters
{
    /// <summary>
    /// Modelo → marcador. Escucha los cambios de saldo y le pasa a la vista el texto ya formateado.
    /// </summary>
    public sealed class BalancePresenter : IDisposable
    {
        private readonly IBalanceView _view;
        private readonly IReadOnlyCurrency _currency;
        private readonly INumberFormatter _formatter;

        public BalancePresenter(IBalanceView view, IReadOnlyCurrency currency, INumberFormatter formatter)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            _currency.BalanceChanged += OnBalanceChanged;
            OnBalanceChanged(_currency.Balance); // Pinta el valor inicial.
        }

        private void OnBalanceChanged(double balance) => _view.SetBalance(_formatter.Format(balance));

        public void Dispose() => _currency.BalanceChanged -= OnBalanceChanged;
    }
}
