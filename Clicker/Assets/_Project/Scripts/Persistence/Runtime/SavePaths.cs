using System.IO;
using UnityEngine;

namespace CookieClicker.Persistence
{
    /// <summary>Rutas de guardado compartidas entre el juego y las herramientas de editor.</summary>
    public static class SavePaths
    {
        public const string DefaultFileName = "save.dat";

        public static string DefaultFilePath => Path.Combine(Application.persistentDataPath, DefaultFileName);
    }
}
