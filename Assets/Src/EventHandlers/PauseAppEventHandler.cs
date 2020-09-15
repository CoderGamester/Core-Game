using Events;
using GameLovers.Services;
using Services;
using UnityEngine;

namespace EventHandlers
{
	/// <summary>
	/// This handles the logic when the game is paused (ex: phone call or goes to phone home screen)
	/// </summary>
	public class PauseAppEventHandler : MonoBehaviour
	{
		private IGameServices _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServices>();
		}

		private void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
			{
				_services?.DataSaver?.SaveAllData();
			}
			
			_services?.MessageBrokerService?.Publish(new ApplicationPausedEvent{ IsPaused = isPaused });
		}
	}
}