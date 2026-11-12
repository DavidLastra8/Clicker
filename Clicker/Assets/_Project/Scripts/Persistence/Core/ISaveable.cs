using System.IO;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Cualquier parte del juego que quiera guardarse implementa esta interfaz y se registra en el ISaveService.
    /// Añadir algo nuevo al guardado = crear una clase nueva, sin tocar el servicio (OCP).
    /// </summary>
    public interface ISaveable
    {
        /// <summary>Identificador único y estable de la sección. No lo cambies una vez publicado.</summary>
        string SaveKey { get; }

        /// <summary>Versión del formato de ESTA sección. Súbela cuando cambies lo que escribe Save.</summary>
        int DataVersion { get; }

        void Save(BinaryWriter writer);

        /// <summary>Lee la sección. dataVersion es la versión con la que se escribió, para migrar datos antiguos.</summary>
        void Load(BinaryReader reader, int dataVersion);
    }
}
