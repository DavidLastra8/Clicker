using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CookieClicker.Views
{
    /// <summary>
    /// Feedback visual opcional: pequeño "rebote" de escala al pulsar.
    /// Va en un componente aparte para que la vista del botón tenga una única responsabilidad.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class ButtonPunchFeedback : MonoBehaviour
    {
        [SerializeField] private RectTransform _target;
        [SerializeField, Range(0.5f, 1f)] private float _pressedScale = 0.9f;
        [SerializeField, Min(0.01f)] private float _duration = 0.12f;

        private Button _button;
        private Vector3 _baseScale;
        private Coroutine _routine;

        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_target == null)
                _target = (RectTransform)transform;
            _baseScale = _target.localScale;
        }

        private void OnEnable() => _button.onClick.AddListener(Play);

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Play);
            _target.localScale = _baseScale;
        }

        private void Play()
        {
            if (_routine != null)
                StopCoroutine(_routine);
            _routine = StartCoroutine(Punch());
        }

        private IEnumerator Punch()
        {
            float elapsed = 0f;
            while (elapsed < _duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / _duration;
                // Baja hasta _pressedScale a mitad de la animación y vuelve a 1.
                float scale = Mathf.Lerp(_pressedScale, 1f, Mathf.Abs(t * 2f - 1f));
                _target.localScale = _baseScale * scale;
                yield return null;
            }
            _target.localScale = _baseScale;
            _routine = null;
        }
    }
}
