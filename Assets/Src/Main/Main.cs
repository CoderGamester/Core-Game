using Data;
using Events;
using GameLovers.Services;
using Logic;
using Newtonsoft.Json;
using Services;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Main
{
	/// <summary>
	/// The Main entry point of the game
	/// </summary>
	public class Main : MonoBehaviour
	{
		[SerializeField] private InputSystemUIInputModule _inputSystem;
		[SerializeField] private Camera _mainCamera;
		
		private GameStateMachine _stateMachine;
		private GameLogic _gameLogic;
		private GameServices _gameServices;

		private void Awake()
		{
			var messageBroker = new MessageBrokerService();
			var timeService = new TimeService();
			var dataService = new DataService();
			var worldObjectReference = new WorldObjectReferenceService(_inputSystem, _mainCamera);

			LoadGameData(timeService, dataService);
			
			_gameLogic = new GameLogic(messageBroker, timeService, dataService);
			_gameServices = new GameServices(messageBroker, timeService, dataService, _gameLogic, worldObjectReference);
			
			MainInstaller.Bind<IGameDataProvider>(_gameLogic);
			MainInstaller.Bind<IGameServices>(_gameServices);
			
			_stateMachine = new GameStateMachine(_gameLogic, _gameServices);
		}

		private void Start()
		{
			_stateMachine.Run();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			_gameServices.MessageBrokerService.Publish(new ApplicationPausedEvent { IsPaused = pauseStatus });

			if (pauseStatus)
			{
				_gameServices.DataSaverService.SaveAllData();
			}
		}

		private void OnApplicationQuit()
		{
			_gameServices.DataSaverService.SaveAllData();
		}

		private void LoadGameData(ITimeService timeService, IDataService dataService)
		{
			var time = timeService.DateTimeUtcNow;
			var appDataJson = PlayerPrefs.GetString(nameof(AppData), "");
			var playerDataJson = PlayerPrefs.GetString(nameof(PlayerData), "");
			var appData = string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson);
			
			dataService.AddData(appData);
			dataService.AddData(string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson));

			if (string.IsNullOrEmpty(appDataJson))
			{
				appData.FirstLoginTime = time;
				appData.LoginTime = time;
			}
			
			appData.LastLoginTime = appData.LoginTime;
			appData.LoginTime = time;
			appData.LoginCount++;
		}
	}
}
