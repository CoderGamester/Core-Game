using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using Unity.Services.Core;
using Cysharp.Threading.Tasks;
using GameLovers.Services;
using GameLovers;
using GameLovers.UiService;
using GameLovers.ConfigsProvider;
using GameLovers.AssetsImporter;
using Game.Logic;
using Game.Services;
using Game.StateMachines;
using Game.Messages;

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
		private GameServicesLocator _services;
		private IGameLogicLocator _gameLogic;
		private IDataService _dataService;
		private Coroutine _pauseCoroutine;
		private bool _onApplicationPauseFlag;
		private bool _onApplicationAlreadyQuitFlag;

		private void Awake()
		{
			var installer = new Installer();
			var worldObjectReference = new WorldObjectReferenceService(_inputSystem, _mainCamera);

			installer.Bind<IMessageBrokerService>(new MessageBrokerService());
			installer.Bind<ITimeService>(new TimeService());
			installer.Bind<GameUiService, IGameUiServiceInit, IGameUiService>(new GameUiService(new UiAssetLoader()));
			installer.Bind<IPoolService>(new PoolService());
			installer.Bind<ITickService>(new TickService());
			installer.Bind<ICoroutineService>(new CoroutineService());
			installer.Bind<AssetResolverService, IAssetResolverService, IAssetAdderService>(new AssetResolverService());
			installer.Bind<ConfigsProvider, IConfigsAdder, IConfigsProvider>(new ConfigsProvider());
			installer.Bind<DataService, IDataService, IDataProvider>(new DataService());
			installer.Bind<IAnalyticsService>(new AnalyticsService(installer.Resolve<IMessageBrokerService>(), installer.Resolve<IDataProvider>()));

			var gameLogic = new GameLogicLocator(installer);

			installer.Bind<IGameLogicLocatorInit>(gameLogic);
			installer.Bind<IGameDataProviderLocator>(gameLogic);
			installer.Bind<ICommandService<IGameLogicLocator>>(new CommandService<IGameLogicLocator>(gameLogic, installer.Resolve<IMessageBrokerService>()));

			var gameServices = new GameServicesLocator(installer);

			installer.Bind<IGameServicesLocator>(gameServices);
			MainInstaller.Bind<IGameDataProviderLocator>(gameLogic);
			MainInstaller.Bind<IGameServicesLocator>(gameServices);

			_dataService = installer.Resolve<IDataService>();
			_gameLogic = gameLogic;
			_services = gameServices;
			_stateMachine = new GameStateMachine(installer);
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskExceptionLogging;

			DontDestroyOnLoad(this);
		}

		private void OnDestroy()
		{
			System.Threading.Tasks.TaskScheduler.UnobservedTaskException -= TaskExceptionLogging;

			MainInstaller.Clean();
		}

		private void Start()
		{
			OnStart().Forget();
		}

		private async UniTask OnStart()
		{			
			//TouchSimulation.Enable();
			EnhancedTouchSupport.Enable();
			InitAtt();

			await UniTask.WhenAll(VersionServices.LoadVersionDataAsync().AsUniTask(), UnityServices.InitializeAsync().AsUniTask());

			_services.Init();
			_stateMachine.Run();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				_dataService.SaveAllData();
			}
		}

		private void OnApplicationPause(bool isPaused)
		{
			// This is mandatory because OnApplicationPause is always called when the GameObject starts after Awake() call -> https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationPause.html
			if (!_onApplicationPauseFlag)
			{
				_onApplicationPauseFlag = true;
				return;
			}

			if (isPaused)
			{
				_dataService.SaveAllData();
				_services.AnalyticsService.FlushEvents();
				
				_pauseCoroutine = StartCoroutine(EndAppCoroutine());
			}
			else if (_pauseCoroutine != null)
			{
				StopCoroutine(_pauseCoroutine);

				_pauseCoroutine = null;
			}

			_services.MessageBrokerService.Publish(new ApplicationPausedMessage { IsPaused = isPaused });

#if UNITY_WEBGL
			// OnApplicationQuit is not invoked on WebGL builds. The alternative is -> https://stackoverflow.com/questions/74295132/unity-webgl-onapplicationquit 
			if (isPaused)
			{
				OnApplicationQuit();
			}
#endif
		}

		private void OnApplicationQuit()
		{
			if (_onApplicationAlreadyQuitFlag) return;

			_onApplicationAlreadyQuitFlag = true;

			_dataService.SaveAllData();
			_services.AnalyticsService.FlushEvents();
			_services.MessageBrokerService.Publish(new ApplicationQuitMessage());
			_services.AnalyticsService.SessionCalls.SessionEnd(_gameLogic.AppLogic.QuitReason);
		}

		private IEnumerator EndAppCoroutine()
		{
			// The app is closed after 30 sec of being unused
			yield return new WaitForSeconds(30);

			OnApplicationQuit();
			_gameLogic.AppLogic.QuitGame("App closed after 30 sec of being unused");
		}

		private void TaskExceptionLogging(object sender, UnobservedTaskExceptionEventArgs e)
		{
			if (sender.GetType().GetGenericTypeDefinition() == typeof(Task<>))
			{
				var task = sender as Task<object>;
				var objName = task.Result is UnityEngine.Object ? ((UnityEngine.Object)task.Result).name : task.Result.ToString();

				Debug.LogError($"Task exception sent by the object {objName}");
			}

			_services.AnalyticsService.ErrorsCalls.CrashLog(e.Exception);
		}

		private void InitAtt()
		{
#if UNITY_IOS
			Unity.Advertisement.IosSupport.SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
			
			if (Unity.Advertisement.IosSupport.ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == 
			    Unity.Advertisement.IosSupport.ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
			{
				Unity.Advertisement.IosSupport.ATTrackingStatusBinding.RequestAuthorizationTracking();
			}
#endif
		}
	}
}
