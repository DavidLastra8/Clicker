using UnityEngine;

namespace CookieClicker.Persistence
{
    /// <summary>
    /// Decide CUÁNDO guardar: cada X segundos, al pasar a segundo plano (móvil) y al cerrar el juego.
    /// Qué y cómo se guarda es cosa del ISaveService.
    /// </summary>
    public sealed class AutoSaveRunner : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float _intervalSeconds = 30f;

        private ISaveService _saveService;
        private float _elapsed;

        public void Initialize(ISaveService saveService)
        {
            _saveService = saveService;
            _elapsed = 0f;
        }

        private void Update()
        {
            if (_saveService == null)
                return;

            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed < _intervalSeconds)
                return;

            _elapsed = 0f;
            _saveService.Save();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                _saveService?.Save();
        }

        private void OnApplicationQuit() => _saveService?.Save();
    }
}
