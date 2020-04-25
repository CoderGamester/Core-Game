using Events;
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
	/// This Presenter handles the HUD UI by:
	/// - Showing the HUD visual status
	/// </summary>
	public class HudPresenter : UiPresenter
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
			_services.MessageBrokerService.Subscribe<CurrencyValueChangedEvent>(OnCurrencyValueChanged);
		}

		protected override void OnOpened()
		{
			_softCurrencyText.text = $"SC: {_dataProvider.CurrencyDataProvider.SoftCurrencyAmount.ToString()}";
			_hardCurrencyText.text = $"HC: {_dataProvider.CurrencyDataProvider.HardCurrencyAmount.ToString()}";
		}

		private void OnCurrencyValueChanged(CurrencyValueChangedEvent eventData)
		{
			if (eventData.Currency == GameId.HardCurrency)
			{
				_hardCurrencyText.text = $"HC: {eventData.NewValue.ToString()}";
			}
			else if (eventData.Currency == GameId.SoftCurrency)
			{
				_softCurrencyText.text = $"SC: {eventData.NewValue.ToString()}";
			}
		}
	}
}