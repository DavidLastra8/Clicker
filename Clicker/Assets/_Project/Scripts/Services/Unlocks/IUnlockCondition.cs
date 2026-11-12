using System;

namespace CookieClicker.Services
{
    /// <summary>Decide si algo (una mejora, un botón...) está disponible.</summary>
    public interface IUnlockCondition
    {
        bool IsUnlocked { get; }
        event Action Changed;
    }
}
