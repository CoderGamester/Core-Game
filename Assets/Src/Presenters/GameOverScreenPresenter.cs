using Game.Messages;
using Game.Services;
using GameLovers.Services;
using GameLovers.UiService;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presenters
{
	/// <summary>
	/// This Presenter handles the Game Over UI by:
	/// - Allowing the user to restart the game by clicking the restart button button
	/// </summary>
	public class GameOverScreenPresenter : UiPresenter
	{
		[SerializeField] private Button _restartButton;
		[SerializeField] private Button _menuButton;

		private IGameServicesLocator _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServicesLocator>();

			_restartButton.onClick.AddListener(Restart);
			_menuButton.onClick.AddListener(ReturnMenu);
		}

		private void ReturnMenu()
		{
			_services.MessageBrokerService.PublishSafe(new OnReturnMenuClickedMessage());
		}

		private void Restart()
		{
			_services.MessageBrokerService.PublishSafe(new OnGameRestartClickedMessage());
		}
	}
}

