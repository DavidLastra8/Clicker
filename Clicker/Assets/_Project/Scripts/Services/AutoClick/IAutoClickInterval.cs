namespace CookieClicker.Services
{
    /// <summary>Segundos entre cada clic automático de los ayudantes.</summary>
    public interface IAutoClickInterval
    {
        double Seconds { get; }
    }
}
