using System;

namespace CookieClicker.Services
{
    public sealed class AlwaysUnlocked : IUnlockCondition
    {
        public static readonly AlwaysUnlocked Instance = new();

        private AlwaysUnlocked() { }

        public bool IsUnlocked => true;

        // Nunca cambia, así que no hace falta guardar suscriptores.
        public event Action Changed { add { } remove { } }
    }
}
