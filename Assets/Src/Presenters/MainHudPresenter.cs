using GameLovers.Services;
using GameLovers.UiService;
using GameLovers;
using Game.Ids;
using Game.Logic;
using Game.Messages;
using Game.Services;
using TMPro;
using UnityEngine;
using Game.Views;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Presenters
{
	/// <summary>
	/// This Presenter handles the Main HUD UI by:
	/// - Showing the HUD visual status
	/// </summary>
	public class MainHudPresenter : UiPresenter<MainHudPresenter.PresenterData>
	{
		public struct PresenterData
		{
			public UnityAction OnPauseClicked;
		}
		
		[SerializeField] private TimerView _timer;
		[SerializeField] private TextMeshProUGUI _softCurrencyText;
		[SerializeField] private TextMeshProUGUI _hardCurrencyText;
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _gameOverCheatButton;

		private IGameDataProviderLocator _dataProvider;
		private IGameServicesLocator _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProviderLocator>();
			_services = MainInstaller.Resolve<IGameServicesLocator>();

			_timer.Init(_services);
			_pauseButton.onClick.AddListener(() => Data.OnPauseClicked.Invoke());
			_gameOverCheatButton.onClick.AddListener(OnGameOverCheatButtonClicked);
		}

		private void OnGameOverCheatButtonClicked()
		{
			_services.MessageBrokerService.Publish(new OnGameOverMessage());
		}

		protected override void OnOpened()
		{
			_dataProvider.CurrencyDataProvider.Currencies.InvokeObserve(GameId.SoftCurrency, OnSoftCurrencyUpdated);
			_dataProvider.CurrencyDataProvider.Currencies.InvokeObserve(GameId.HardCurrency, OnHardCurrencyUpdated);
		}

		private void OnSoftCurrencyUpdated(GameId currency, int amountBefore, int amountAfter, ObservableUpdateType updateType)
		{
			_softCurrencyText.text = $"SC: {amountAfter.ToString()}";
		}

		private void OnHardCurrencyUpdated(GameId currency, int amountBefore, int amountAfter, ObservableUpdateType updateType)
		{
			_hardCurrencyText.text = $"HC: {amountAfter.ToString()}";
		}
	}
}