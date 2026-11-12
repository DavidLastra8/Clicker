using System;
using CookieClicker.Models;

namespace CookieClicker.Services
{
    /// <summary>Se desbloquea cuando otra mejora llega a cierto nivel.</summary>
    public sealed class UpgradeLevelUnlockCondition : IUnlockCondition, IDisposable
    {
        private readonly IReadOnlyUpgrade _required;
        private readonly int _requiredLevel;

        public event Action Changed;

        public UpgradeLevelUnlockCondition(IReadOnlyUpgrade required, int requiredLevel)
        {
            _required = required ?? throw new ArgumentNullException(nameof(required));
            _requiredLevel = Math.Max(1, requiredLevel);
            _required.LevelChanged += OnRequiredLevelChanged;
        }

        public bool IsUnlocked => _required.Level >= _requiredLevel;

        private void OnRequiredLevelChanged() => Changed?.Invoke();

        public void Dispose() => _required.LevelChanged -= OnRequiredLevelChanged;
    }
}
