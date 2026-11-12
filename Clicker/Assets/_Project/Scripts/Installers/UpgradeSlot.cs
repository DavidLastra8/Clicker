using System;
using CookieClicker.Config;
using CookieClicker.Views;
using UnityEngine;

namespace CookieClicker.Installers
{
    /// <summary>Pareja configuración + botón para el Inspector del GameInstaller.</summary>
    [Serializable]
    public struct UpgradeSlot
    {
        [SerializeField] private UpgradeDefinition _definition;
        [SerializeField] private UpgradeButtonView _view;

        public UpgradeDefinition Definition => _definition;
        public UpgradeButtonView View => _view;
    }
}
