namespace CookieClicker.Models
{
    /// <summary>
    /// Permite gastar dinero (tienda de mejoras). Todavía no se usa, pero el modelo ya lo soporta.
    /// </summary>
    public interface ICurrencySpender
    {
        bool TrySpend(double amount);
    }
}
