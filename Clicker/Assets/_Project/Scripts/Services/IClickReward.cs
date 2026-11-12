namespace CookieClicker.Services
{
    /// <summary>
    /// Cuánto dinero da un clic. Abstraído para que las futuras mejoras (x2, +5 por clic...)
    /// se añadan con nuevas implementaciones/decoradores sin tocar el presenter (OCP).
    /// </summary>
    public interface IClickReward
    {
        double GetAmount();
    }
}
