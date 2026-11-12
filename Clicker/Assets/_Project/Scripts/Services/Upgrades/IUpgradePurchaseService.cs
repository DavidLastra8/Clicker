using CookieClicker.Models;

namespace CookieClicker.Services
{
    public interface IUpgradePurchaseService
    {
        /// <summary>Cobra el coste actual y sube un nivel. Devuelve false si no hay dinero o está al máximo.</summary>
        bool TryPurchase(IPurchasableUpgrade upgrade);
    }
}
