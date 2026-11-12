using System;

namespace CookieClicker.Views
{
    /// <summary>Vista pasiva de un botón de mejora.</summary>
    public interface IUpgradeView
    {
        event Action PurchaseClicked;

        void SetTitle(string title);
        void SetDescription(string description);
        void SetLevel(int level);
        void SetCost(string cost);
        void SetInteractable(bool interactable);
        void SetVisible(bool visible);
    }
}
