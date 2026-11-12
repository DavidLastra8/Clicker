using System;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>Une el dinero con las mejoras: el modelo de mejora no toca el dinero y el dinero no sabe de mejoras.</summary>
    public sealed class UpgradePurchaseService : IUpgradePurchaseService
    {
        private readonly ICurrencySpender _wallet;

        public UpgradePurchaseService(ICurrencySpender wallet)
        {
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        }

        public bool TryPurchase(IPurchasableUpgrade upgrade)
        {
            if (upgrade == null)
                throw new ArgumentNullException(nameof(upgrade));

            if (upgrade.IsMaxed || !_wallet.TrySpend(upgrade.CurrentCost))
                return false;

            upgrade.LevelUp();
            return true;
        }
    }
}
