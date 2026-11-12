using UnityEngine;

namespace CookieClicker.Config
{
    /// <summary>
    /// Configuración de una mejora, editable desde el Inspector.
    /// Crear: clic derecho en Project > Create > Clicker > Upgrade Definition.
    /// </summary>
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Clicker/Upgrade Definition")]
    public sealed class UpgradeDefinition : ScriptableObject
    {
        [Header("Identificación")]
        [Tooltip("Clave única usada en el guardado. No la cambies una vez haya partidas guardadas.")]
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField, TextArea] private string _description;

        [Header("Coste")]
        [Tooltip("Precio de la primera compra.")]
        [SerializeField] private double _baseCost = 10d;
        [Tooltip("Cuánto se multiplica el precio en cada compra. 1.15 = +15 % por compra.")]
        [SerializeField] private double _costMultiplier = 1.15d;
        [Tooltip("Nivel máximo. 0 = sin límite.")]
        [SerializeField, Min(0)] private int _maxLevel;

        [Header("Efecto")]
        [Tooltip("Cuánto aporta cada nivel. Su significado depende de la mejora (ver el tooltip del GameInstaller).")]
        [SerializeField] private double _effectPerLevel = 1d;

        [Header("Desbloqueo (opcional)")]
        [Tooltip("Si se asigna, esta mejora está oculta hasta que la mejora indicada llegue al nivel requerido.")]
        [SerializeField] private UpgradeDefinition _requiredUpgrade;
        [SerializeField, Min(1)] private int _requiredLevel = 1;

        public string Id => _id;
        public string Title => _title;
        public string Description => _description;
        public double BaseCost => _baseCost;
        public double CostMultiplier => _costMultiplier;
        public int MaxLevel => _maxLevel;
        public double EffectPerLevel => _effectPerLevel;
        public UpgradeDefinition RequiredUpgrade => _requiredUpgrade;
        public int RequiredLevel => _requiredLevel;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_id))
                _id = name;
            if (_baseCost < 1d)
                _baseCost = 1d;
            if (_costMultiplier < 1d)
                _costMultiplier = 1d;
            if (_requiredUpgrade == this)
                _requiredUpgrade = null;
        }
    }
}
