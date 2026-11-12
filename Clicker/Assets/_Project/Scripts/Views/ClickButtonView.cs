using System;
using UnityEngine;
using UnityEngine.UI;

namespace CookieClicker.Views
{
    /// <summary>
    /// Botón principal (la "galleta"). No sabe nada de dinero: traduce el onClick de Unity a un evento C#.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class ClickButtonView : MonoBehaviour, IClickButtonView
    {
        [SerializeField] private Button _button;

        public event Action Clicked;

        private void Reset() => _button = GetComponent<Button>();

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }

        private void OnEnable() => _button.onClick.AddListener(HandleClick);

        private void OnDisable() => _button.onClick.RemoveListener(HandleClick);

        private void HandleClick() => Clicked?.Invoke();
    }
}
