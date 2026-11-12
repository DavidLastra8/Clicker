using System;

namespace CookieClicker.Views
{
    /// <summary>Vista pasiva del botón principal: solo avisa de que se ha pulsado.</summary>
    public interface IClickButtonView
    {
        event Action Clicked;
    }
}
