namespace CookieClicker.Models
{
    /// <summary>
    /// Permite sumar dinero (clics, producción pasiva...). Separado de gastar (ISP).
    /// </summary>
    public interface ICurrencyReceiver
    {
        void Add(double amount);
    }
}
