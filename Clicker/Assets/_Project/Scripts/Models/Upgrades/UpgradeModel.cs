using System;
using CookieClicker.Services;

namespace CookieClicker.Models
{
    /// <summary>
    /// Nivel de una mejora. No sabe qué efecto tiene ni cuánto dinero hay: eso lo deciden otras clases.
    /// </summary>
    public sealed class UpgradeModel : IPurchasableUpgrade, IRestorableUpgrade
    {
        private readonly ICostFormula _costFormula;

        public string Id { get; }
        public int MaxLevel { get; }
        public int Level { get; private set; }
        public bool IsMaxed => MaxLevel > 0 && Level >= MaxLevel;
        public double CurrentCost => _costFormula.GetCost(Level);

        public event Action LevelChanged;

        public UpgradeModel(string id, ICostFormula costFormula, int maxLevel = 0)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("La mejora necesita un id.", nameof(id));
            if (maxLevel < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLevel));

            Id = id;
            _costFormula = costFormula ?? throw new ArgumentNullException(nameof(costFormula));
            MaxLevel = maxLevel;
        }

        public void LevelUp()
        {
            if (IsMaxed)
                throw new InvalidOperationException($"La mejora '{Id}' ya está al nivel máximo.");

            Level++;
            LevelChanged?.Invoke();
        }

        public void RestoreLevel(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            Level = MaxLevel > 0 ? Math.Min(level, MaxLevel) : level;
            LevelChanged?.Invoke();
        }
    }
}
