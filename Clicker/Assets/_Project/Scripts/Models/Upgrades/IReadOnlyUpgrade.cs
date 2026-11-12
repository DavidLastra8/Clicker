using System;

namespace CookieClicker.Models
{
    /// <summary>Estado de una mejora visto desde fuera (efectos, vistas, condiciones de desbloqueo).</summary>
    public interface IReadOnlyUpgrade
    {
        string Id { get; }
        int Level { get; }

        /// <summary>0 = sin límite.</summary>
        int MaxLevel { get; }
        bool IsMaxed { get; }
        double CurrentCost { get; }

        event Action LevelChanged;
    }
}
