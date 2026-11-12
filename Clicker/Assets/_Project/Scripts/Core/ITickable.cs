namespace CookieClicker.Core
{
    /// <summary>Lógica que necesita avanzar con el tiempo sin ser un MonoBehaviour.</summary>
    public interface ITickable
    {
        void Tick(float deltaTime);
    }
}
