namespace CookieClicker.Models
{
    /// <summary>Mejora que se puede subir de nivel (lo usa el servicio de compra).</summary>
    public interface IPurchasableUpgrade : IReadOnlyUpgrade
    {
        void LevelUp();
    }
}
