using System;
using System.Collections.Generic;
using CookieClicker.Config;
using CookieClicker.Models;
using CookieClicker.Persistence;
using CookieClicker.Presenters;
using CookieClicker.Runtime;
using CookieClicker.Services;
using CookieClicker.Views;
using UnityEngine;

namespace CookieClicker.Installers
{
    /// <summary>
    /// Composition root: el único sitio que crea las piezas concretas y las conecta.
    /// Si más adelante usas VContainer/Zenject, este script es lo único que cambia.
    /// </summary>
    public sealed class GameInstaller : MonoBehaviour
    {
        [Header("Vistas")]
        [SerializeField] private ClickButtonView _clickButtonView;
        [SerializeField] private BalanceView _balanceView;

        [Header("Configuración")]
        [SerializeField, Min(0.0001f)] private double _moneyPerClick = 1d;
        [SerializeField, Min(0f)] private double _startingMoney = 0d;

        [Header("Mejoras")]
        [Tooltip("Efecto por nivel = dinero extra por clic.")]
        [SerializeField] private UpgradeSlot _clickPower;
        [Tooltip("Efecto por nivel = multiplicador extra (0.5 = +50 % por nivel).")]
        [SerializeField] private UpgradeSlot _clickMultiplier;
        [Tooltip("Efecto por nivel = ayudantes que se añaden en cada compra.")]
        [SerializeField] private UpgradeSlot _helper;
        [Tooltip("Efecto por nivel = reducción del intervalo de los ayudantes (0.1 = -10 % por nivel).")]
        [SerializeField] private UpgradeSlot _helperSpeed;

        [Header("Ayudantes")]
        [Tooltip("Segundos entre clics de los ayudantes sin mejoras de velocidad.")]
        [SerializeField, Min(0.01f)] private float _helperBaseInterval = 2f;
        [Tooltip("El intervalo nunca bajará de este valor.")]
        [SerializeField, Min(0.01f)] private float _helperMinInterval = 0.1f;

        [Header("Runtime (se añaden solos si se dejan vacíos)")]
        [SerializeField] private AutoSaveRunner _autoSaveRunner;
        [SerializeField] private TickRunner _tickRunner;

        private readonly List<IDisposable> _disposables = new();
        private readonly Dictionary<UpgradeDefinition, UpgradeModel> _upgradesByDefinition = new();
        private ISaveService _saveService;

        private void Awake()
        {
            ValidateSlots();

            // Modelos
            var currency = new CurrencyModel(_startingMoney);
            var clickPower = CreateUpgrade(_clickPower);
            var clickMultiplier = CreateUpgrade(_clickMultiplier);
            var helper = CreateUpgrade(_helper);
            var helperSpeed = CreateUpgrade(_helperSpeed);

            // Guardado: registra aquí cada nueva sección que quieras persistir.
            _saveService = new BinarySaveService(new FileSaveStorage(SavePaths.DefaultFilePath), Debug.LogException);
            _saveService.Register(new CurrencySaveHandler(currency, currency));
            _saveService.Register(new UpgradesSaveHandler(_upgradesByDefinition.Values));
            _saveService.Load();

            // Recompensa por clic: base -> + bonus por nivel -> x multiplicador.
            IClickReward reward = new FixedClickReward(_moneyPerClick);
            reward = new FlatBonusClickReward(reward, clickPower, _clickPower.Definition.EffectPerLevel);
            reward = new MultiplierClickReward(reward, clickMultiplier, _clickMultiplier.Definition.EffectPerLevel);

            // Ayudantes
            var interval = new UpgradeableAutoClickInterval(_helperBaseInterval, _helperMinInterval,
                helperSpeed, _helperSpeed.Definition.EffectPerLevel);
            var autoClicker = new AutoClickerService(helper, _helper.Definition.EffectPerLevel, interval, reward, currency);

            // Servicios
            var formatter = new AbbreviatedNumberFormatter();
            var purchaseService = new UpgradePurchaseService(currency);

            // Presenters
            _disposables.Add(new ClickPresenter(_clickButtonView, currency, reward));
            _disposables.Add(new BalancePresenter(_balanceView, currency, formatter));
            foreach (var slot in new[] { _clickPower, _clickMultiplier, _helper, _helperSpeed })
                _disposables.Add(CreateUpgradePresenter(slot, purchaseService, currency, formatter));

            // Runtime
            GetOrAdd(ref _tickRunner).Register(autoClicker);
            GetOrAdd(ref _autoSaveRunner).Initialize(_saveService);
        }

        private UpgradeModel CreateUpgrade(UpgradeSlot slot)
        {
            var definition = slot.Definition;
            var model = new UpgradeModel(definition.Id,
                new ExponentialCostFormula(definition.BaseCost, definition.CostMultiplier), definition.MaxLevel);
            _upgradesByDefinition.Add(definition, model);
            return model;
        }

        private UpgradePresenter CreateUpgradePresenter(UpgradeSlot slot, IUpgradePurchaseService purchaseService,
            IReadOnlyCurrency currency, INumberFormatter formatter)
        {
            var definition = slot.Definition;
            return new UpgradePresenter(slot.View, _upgradesByDefinition[definition], purchaseService,
                currency, formatter, CreateUnlockCondition(definition), definition.Title, definition.Description);
        }

        private IUnlockCondition CreateUnlockCondition(UpgradeDefinition definition)
        {
            if (definition.RequiredUpgrade == null)
                return AlwaysUnlocked.Instance;

            if (!_upgradesByDefinition.TryGetValue(definition.RequiredUpgrade, out var required))
                throw new InvalidOperationException(
                    $"La mejora '{definition.name}' requiere '{definition.RequiredUpgrade.name}', que no está asignada en el GameInstaller.");

            var condition = new UpgradeLevelUnlockCondition(required, definition.RequiredLevel);
            _disposables.Add(condition);
            return condition;
        }

        private void ValidateSlots()
        {
            Validate(_clickPower, nameof(_clickPower));
            Validate(_clickMultiplier, nameof(_clickMultiplier));
            Validate(_helper, nameof(_helper));
            Validate(_helperSpeed, nameof(_helperSpeed));

            static void Validate(UpgradeSlot slot, string field)
            {
                if (slot.Definition == null || slot.View == null)
                    throw new InvalidOperationException($"GameInstaller: asigna Definition y View en '{field}'.");
            }
        }

        private T GetOrAdd<T>(ref T component) where T : Component
        {
            if (component == null && !TryGetComponent(out component))
                component = gameObject.AddComponent<T>();
            return component;
        }

        private void OnDestroy()
        {
            // Los presenters se liberan antes que las condiciones de las que dependen.
            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i].Dispose();
            _disposables.Clear();
        }
    }
}
