using Game.Messages;
using Game.Services;
using GameLovers.Services;
using GameLovers.UiService;
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

		private IGameServices _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServices>();

			_restartButton.onClick.AddListener(Restart);
		}

		private void Restart()
		{
			_services.MessageBrokerService.Publish(new OnGameRestartClickedMessage());
		}
	}
}

