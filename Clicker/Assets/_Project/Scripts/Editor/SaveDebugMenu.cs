using System.IO;
using CookieClicker.Persistence;
using UnityEditor;
using UnityEngine;

namespace CookieClicker.EditorTools
{
    /// <summary>Atajos de editor para probar el guardado.</summary>
    public static class SaveDebugMenu
    {
        [MenuItem("Tools/Clicker/Borrar guardado")]
        private static void DeleteSave()
        {
            if (!EditorUtility.DisplayDialog("Borrar guardado",
                    $"¿Borrar el archivo de guardado?\n{SavePaths.DefaultFilePath}", "Borrar", "Cancelar"))
                return;

            new FileSaveStorage(SavePaths.DefaultFilePath).Delete();
            Debug.Log("Guardado borrado.");
        }

        [MenuItem("Tools/Clicker/Abrir carpeta de guardado")]
        private static void OpenSaveFolder()
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            EditorUtility.RevealInFinder(SavePaths.DefaultFilePath);
        }
    }
}
