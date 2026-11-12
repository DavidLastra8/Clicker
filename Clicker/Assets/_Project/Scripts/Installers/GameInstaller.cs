using System;
using System.Collections.Generic;
using CookieClicker.Models;
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

        private readonly List<IDisposable> _disposables = new();

        private void Awake()
        {
            var currency = new CurrencyModel(_startingMoney);
            var reward = new FixedClickReward(_moneyPerClick);
            var formatter = new AbbreviatedNumberFormatter();

            _disposables.Add(new ClickPresenter(_clickButtonView, currency, reward));
            _disposables.Add(new BalancePresenter(_balanceView, currency, formatter));
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
            _disposables.Clear();
        }
    }
}
