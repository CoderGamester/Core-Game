using Data;
using EventHandlers;
using Events;
using GameLovers.GoogleSheetImporter;
using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using Newtonsoft.Json;
using Services;
using StateMachines;
using UnityEngine;
using UnityEngine.InputSystem.UI;

// ReSharper disable once CheckNamespace

namespace Game
{
	/// <summary>
	/// The Main entry point of the game
	/// </summary>
	public class Main : MonoBehaviour
	{
		[SerializeField] private InputSystemUIInputModule _inputSystem;
		[SerializeField] private Camera _mainCamera;
		
		private GameStateMachine _stateMachine;

		private void Awake()
		{
			var messageBroker = new MessageBrokerService();
			var timeService = new TimeService();
			var dataService = new DataService();
			var configsProvider = new ConfigsProvider();
			var uiService = new GameUiService(new UiAssetLoader());
			var worldObjectReference = new WorldObjectReferenceService(_inputSystem, _mainCamera);
			var gameLogic = new GameLogic(messageBroker, timeService, dataService, configsProvider);
			var gameServices = new GameServices(messageBroker, timeService, dataService, gameLogic, worldObjectReference);
			
			MainInstaller.Bind<IGameDataProvider>(gameLogic);
			MainInstaller.Bind<IGameServices>(gameServices);
			
			_stateMachine = new GameStateMachine(gameLogic, gameServices, uiService, configsProvider, dataService);
		}

		private void Start()
		{
			var handlerHost = new GameObject("EventHandlers");
			
			handlerHost.AddComponent<PauseAppEventHandler>();
			handlerHost.AddComponent<QuitAppEventHandler>();
			DontDestroyOnLoad(handlerHost);
			_stateMachine.Run();
		}
	}
}
