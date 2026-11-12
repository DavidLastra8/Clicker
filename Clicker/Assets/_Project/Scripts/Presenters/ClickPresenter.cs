using System;
using CookieClicker.Models;
using CookieClicker.Services;
using CookieClicker.Views;

namespace CookieClicker.Presenters
{
    /// <summary>
    /// Botón principal → modelo. Al pulsar, suma al dinero lo que indique la recompensa por clic.
    /// Depende solo de abstracciones (DIP).
    /// </summary>
    public sealed class ClickPresenter : IDisposable
    {
        private readonly IClickButtonView _view;
        private readonly ICurrencyReceiver _currency;
        private readonly IClickReward _reward;

        public ClickPresenter(IClickButtonView view, ICurrencyReceiver currency, IClickReward reward)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
            _reward = reward ?? throw new ArgumentNullException(nameof(reward));

            _view.Clicked += OnClicked;
        }

        private void OnClicked() => _currency.Add(_reward.GetAmount());

        public void Dispose() => _view.Clicked -= OnClicked;
    }
}
