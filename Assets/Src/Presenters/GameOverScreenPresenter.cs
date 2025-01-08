using Game.Commands;
using Game.Services;
using GameLovers.Services;
using GameLovers.UiService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Presenters
{
	/// <summary>
	/// This Presenter handles the Game Over Screen by:
	/// - Allowing the user to restart the game
	/// </summary>
	public class GameOverScreenPresenter : UiPresenter
	{
		[SerializeField] private Button _restartButton;

		private IGameServicesLocator _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServicesLocator>();

			_restartButton.onClick.AddListener(Restart);
		}

		private void Restart()
		{
			_services.CommandService.ExecuteCommand(new RestartGameCommand());
		}
	}
}

