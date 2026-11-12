namespace CookieClicker.Services
{
    /// <summary>Calcula el precio de una mejora según el nivel que ya tiene.</summary>
    public interface ICostFormula
    {
        double GetCost(int currentLevel);
    }
}
