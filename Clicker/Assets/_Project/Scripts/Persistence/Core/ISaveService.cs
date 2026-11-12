namespace CookieClicker.Persistence
{
    public interface ISaveService
    {
        bool HasSave { get; }

        void Register(ISaveable saveable);
        void Unregister(ISaveable saveable);

        /// <summary>Guarda todas las secciones registradas. Devuelve false si algo falla.</summary>
        bool Save();

        /// <summary>Carga el archivo y reparte cada sección a su ISaveable. Devuelve false si no hay guardado o es inválido.</summary>
        bool Load();

        void Delete();
    }
}
