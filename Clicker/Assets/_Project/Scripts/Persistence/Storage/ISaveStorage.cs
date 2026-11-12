namespace CookieClicker.Persistence
{
    /// <summary>
    /// Dónde se guardan los bytes (archivo local, nube, PlayerPrefs...).
    /// El servicio no sabe nada del medio físico (DIP).
    /// </summary>
    public interface ISaveStorage
    {
        bool Exists { get; }
        byte[] Read();
        void Write(byte[] data);
        void Delete();
    }
}
