namespace CookieClicker.Services
{
    /// <summary>Convierte un número en texto para mostrarlo. La vista no decide el formato.</summary>
    public interface INumberFormatter
    {
        string Format(double value);
    }
}
