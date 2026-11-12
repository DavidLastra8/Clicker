using System;

namespace CookieClicker.Services
{
    /// <summary>
    /// Coste = baseCost * multiplier ^ nivel, redondeado hacia arriba.
    /// Es la curva clásica de los incrementales: con multiplier 1.15 cada compra cuesta un 15 % más.
    /// </summary>
    public sealed class ExponentialCostFormula : ICostFormula
    {
        private readonly double _baseCost;
        private readonly double _multiplier;

        public ExponentialCostFormula(double baseCost, double multiplier)
        {
            if (baseCost <= 0d)
                throw new ArgumentOutOfRangeException(nameof(baseCost), "El coste base debe ser positivo.");
            if (multiplier < 1d)
                throw new ArgumentOutOfRangeException(nameof(multiplier), "El multiplicador de coste debe ser >= 1.");

            _baseCost = baseCost;
            _multiplier = multiplier;
        }

        public double GetCost(int currentLevel) => Math.Ceiling(_baseCost * Math.Pow(_multiplier, currentLevel));
    }
}
