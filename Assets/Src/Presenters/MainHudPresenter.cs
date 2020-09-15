using System;
using Events;
using GameLovers;
using GameLovers.Services;
using GameLovers.UiService;
using Ids;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using Views;

namespace Presenters
{
	/// <summary>
	/// This Presenter handles the Main HUD UI by:
	/// - Showing the HUD visual status
	/// </summary>
	public class MainHudPresenter : UiPresenter
	{
		[SerializeField] private TimerView _timer;
		[SerializeField] private TextMeshProUGUI _softCurrencyText;
		[SerializeField] private TextMeshProUGUI _hardCurrencyText;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();

			_timer.Init(_services);
		}

		protected override void OnOpened()
		{
			_dataProvider.CurrencyDataProvider.Currencies.InvokeObserve(GameId.SoftCurrency, ObservableUpdateType.Updated, OnSoftCurrencyUpdated);
			_dataProvider.CurrencyDataProvider.Currencies.InvokeObserve(GameId.HardCurrency, ObservableUpdateType.Updated, OnHardCurrencyUpdated);
		}

		private void OnSoftCurrencyUpdated(GameId currency, int amount)
		{
			_softCurrencyText.text = $"SC: {amount.ToString()}";
		}

		private void OnHardCurrencyUpdated(GameId currency, int amount)
		{
			_hardCurrencyText.text = $"HC: {amount.ToString()}";
		}
	}
}