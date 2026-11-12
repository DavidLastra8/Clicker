namespace CookieClicker.Models
{
    /// <summary>
    /// Permite restaurar el estado del dinero desde un guardado.
    /// Separado del resto para que solo el sistema de guardado pueda sobrescribir el saldo (ISP).
    /// </summary>
    public interface ICurrencyRestorer
    {
        void Restore(double balance, double totalEarned);
    }
}
