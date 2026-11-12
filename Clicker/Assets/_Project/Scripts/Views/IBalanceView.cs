namespace CookieClicker.Views
{
    /// <summary>Vista pasiva del marcador: solo pinta el texto que le da el presenter.</summary>
    public interface IBalanceView
    {
        void SetBalance(string formattedBalance);
    }
}
