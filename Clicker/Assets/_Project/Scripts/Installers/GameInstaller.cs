using System;
using System.Collections.Generic;
using CookieClicker.Models;
using CookieClicker.Persistence;
using CookieClicker.Presenters;
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

        [Header("Guardado")]
        [SerializeField] private AutoSaveRunner _autoSaveRunner;

        private readonly List<IDisposable> _disposables = new();
        private ISaveService _saveService;

        private void Awake()
        {
            // Modelos
            var currency = new CurrencyModel(_startingMoney);

            // Guardado: registra aquí cada nueva sección que quieras persistir.
            _saveService = new BinarySaveService(new FileSaveStorage(SavePaths.DefaultFilePath), Debug.LogException);
            _saveService.Register(new CurrencySaveHandler(currency, currency));
            _saveService.Load();

            // Servicios
            var reward = new FixedClickReward(_moneyPerClick);
            var formatter = new AbbreviatedNumberFormatter();

            // Presenters
            _disposables.Add(new ClickPresenter(_clickButtonView, currency, reward));
            _disposables.Add(new BalancePresenter(_balanceView, currency, formatter));

            if (_autoSaveRunner == null && !TryGetComponent(out _autoSaveRunner))
                _autoSaveRunner = gameObject.AddComponent<AutoSaveRunner>();
            _autoSaveRunner.Initialize(_saveService);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
            _disposables.Clear();
        }
    }
}
