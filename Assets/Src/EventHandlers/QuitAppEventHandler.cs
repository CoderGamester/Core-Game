using GameLovers.Services;
using Services;
using UnityEngine;

namespace EventHandlers
{
	/// <summary>
	/// This handles the logic when the player quits explicitly the app on the device
	/// </summary>
	public class QuitAppEventHandler : MonoBehaviour
	{
		private IGameServices _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServices>();
		}
		
		private void OnApplicationQuit()
		{
			_services?.DataSaver?.SaveAllData();
		}
	}
}