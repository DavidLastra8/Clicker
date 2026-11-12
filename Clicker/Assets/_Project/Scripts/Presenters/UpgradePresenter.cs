using System;
using CookieClicker.Models;
using CookieClicker.Services;
using CookieClicker.Views;

namespace CookieClicker.Presenters
{
    /// <summary>
    /// Un presenter por botón de mejora: muestra nivel y coste, activa el botón si hay dinero
    /// y lo oculta mientras la condición de desbloqueo no se cumpla.
    /// </summary>
    public sealed class UpgradePresenter : IDisposable
    {
        private const string MaxedLabel = "MAX";

        private readonly IUpgradeView _view;
        private readonly IPurchasableUpgrade _upgrade;
        private readonly IUpgradePurchaseService _purchaseService;
        private readonly IReadOnlyCurrency _currency;
        private readonly INumberFormatter _formatter;
        private readonly IUnlockCondition _unlock;

        public UpgradePresenter(IUpgradeView view, IPurchasableUpgrade upgrade, IUpgradePurchaseService purchaseService,
            IReadOnlyCurrency currency, INumberFormatter formatter, IUnlockCondition unlock,
            string title, string description)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _upgrade = upgrade ?? throw new ArgumentNullException(nameof(upgrade));
            _purchaseService = purchaseService ?? throw new ArgumentNullException(nameof(purchaseService));
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _unlock = unlock ?? throw new ArgumentNullException(nameof(unlock));

            _view.SetTitle(title);
            _view.SetDescription(description);

            _view.PurchaseClicked += OnPurchaseClicked;
            _upgrade.LevelChanged += Refresh;
            _currency.BalanceChanged += OnBalanceChanged;
            _unlock.Changed += Refresh;

            Refresh();
        }

        private void OnPurchaseClicked()
        {
            if (_unlock.IsUnlocked)
                _purchaseService.TryPurchase(_upgrade);
        }

        private void OnBalanceChanged(double _) => RefreshAffordability();

        private void Refresh()
        {
            _view.SetVisible(_unlock.IsUnlocked);
            _view.SetLevel(_upgrade.Level);
            _view.SetCost(_upgrade.IsMaxed ? MaxedLabel : _formatter.Format(_upgrade.CurrentCost));
            RefreshAffordability();
        }

        private void RefreshAffordability()
        {
            _view.SetInteractable(!_upgrade.IsMaxed && _currency.Balance >= _upgrade.CurrentCost);
        }

        public void Dispose()
        {
            _view.PurchaseClicked -= OnPurchaseClicked;
            _upgrade.LevelChanged -= Refresh;
            _currency.BalanceChanged -= OnBalanceChanged;
            _unlock.Changed -= Refresh;
        }
    }
}
