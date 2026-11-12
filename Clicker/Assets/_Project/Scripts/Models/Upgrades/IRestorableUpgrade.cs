namespace CookieClicker.Models
{
    /// <summary>Mejora cuyo nivel se puede restaurar desde un guardado.</summary>
    public interface IRestorableUpgrade : IReadOnlyUpgrade
    {
        void RestoreLevel(int level);
    }
}
