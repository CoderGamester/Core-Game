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
			var dataProvider = new DataProviderLogic();
			var worldObjectReference = new WorldObjectReferenceService(_inputSystem, _mainCamera);
			
			LoadData(dataProvider, timeService);
			
			_gameLogic = new GameLogic(messageBroker, dataProvider, timeService);
			_gameServices = new GameServices(messageBroker, timeService, _gameLogic, worldObjectReference);
			
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
				_gameLogic.DataProviderLogic.FlushData();
			}
		}

		private void OnApplicationQuit()
		{
			_gameLogic.DataProviderLogic.FlushData();
		}

		private void LoadData(IDataProviderInternalLogic dataProviderLogic, ITimeService timeService)
		{
			var time = timeService.DateTimeUtcNow;
			var appDataJson = PlayerPrefs.GetString(nameof(AppData), "");
			var playerDataJson = PlayerPrefs.GetString(nameof(PlayerData), "");
			
			dataProviderLogic.AddData(string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson));

			if (string.IsNullOrEmpty(appDataJson))
			{
				dataProviderLogic.AppData.FirstLoginTime = time;
				dataProviderLogic.AppData.LoginTime = time;
			}
			
			dataProviderLogic.AppData.LastLoginTime = dataProviderLogic.AppData.LoginTime;
			dataProviderLogic.AppData.LoginTime = time;
			dataProviderLogic.AppData.LoginCount++;
		}
	}
}
