using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieClicker.Views
{
    /// <summary>
    /// Botón de mejora. Los textos son opcionales: asigna solo los que tenga tu diseño.
    /// Déjalo ACTIVO en la escena aunque empiece oculto: el presenter lo oculta al arrancar.
    /// </summary>
    public sealed class UpgradeButtonView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private Button _button;

        [Header("Textos (opcionales)")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _costText;

        [Header("Plantillas")]
        [SerializeField] private string _levelTemplate = "Nv. {0}";
        [SerializeField] private string _costTemplate = "{0} €";

        public event Action PurchaseClicked;

        private void Reset() => _button = GetComponentInChildren<Button>();

        private void Awake()
        {
            if (_button == null)
                _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick() => PurchaseClicked?.Invoke();

        public void SetTitle(string title) => SetText(_titleText, title);
        public void SetDescription(string description) => SetText(_descriptionText, description);
        public void SetLevel(int level) => SetText(_levelText, string.Format(_levelTemplate, level));
        public void SetCost(string cost) => SetText(_costText, string.Format(_costTemplate, cost));
        public void SetInteractable(bool interactable) => _button.interactable = interactable;
        public void SetVisible(bool visible) => gameObject.SetActive(visible);

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null)
                target.text = value;
        }
    }
}
