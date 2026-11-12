using TMPro;
using UnityEngine;

namespace CookieClicker.Views
{
    /// <summary>Muestra el dinero actual en un texto de TextMeshPro.</summary>
    public sealed class BalanceView : MonoBehaviour, IBalanceView
    {
        [SerializeField] private TMP_Text _balanceText;

        [Tooltip("Plantilla del texto. {0} se sustituye por el número formateado.")]
        [SerializeField] private string _template = "{0} €";

        private void Reset() => _balanceText = GetComponent<TMP_Text>();

        public void SetBalance(string formattedBalance)
        {
            _balanceText.text = string.Format(_template, formattedBalance);
        }
    }
}
