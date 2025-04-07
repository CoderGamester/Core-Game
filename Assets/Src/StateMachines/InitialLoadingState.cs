using System;
using Game.Configs;
using Game.Data;
using GameLovers.ConfigsProvider;
using GameLovers.Services;
using GameLovers.StatechartMachine;
using GameLovers.UiService;
using Game.Ids;
using Game.Logic;
using Game.Services;
using UnityEngine;
using Game.Commands;
using Cysharp.Threading.Tasks;
using Game.Messages;
using Game.Presenters;
using Game.Utils;
using GameLovers.AssetsImporter;

namespace Game.StateMachines
{
	/// <summary>
	/// This class represents the Loading state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	internal class InitialLoadingState
	{
		private static readonly IStatechartEvent _compliance_Clicked_Event = new StatechartEvent("Compliance Accept Clicked Event");
		
		private readonly IGameServicesLocator _services;
		private readonly IGameLogicLocatorInit _gameLogic;
		private readonly IGameUiServiceInit _uiService;
		private readonly IConfigsAdder _configsAdder;
		private readonly IDataService _dataService;
		private readonly IAssetAdderService _assetAdderService;
		private readonly Action<IStatechartEvent> _statechartTrigger;

		public InitialLoadingState(IInstaller installer, Action<IStatechartEvent> statechartTrigger)
		{
			_gameLogic = installer.Resolve<IGameLogicLocatorInit>();
			_services = installer.Resolve<IGameServicesLocator>();
			_uiService = installer.Resolve<IGameUiServiceInit>();
			_configsAdder = installer.Resolve<IConfigsAdder>();
			_dataService = installer.Resolve<IDataService>();
			_assetAdderService = installer.Resolve<IAssetAdderService>();
			_statechartTrigger = statechartTrigger;
		}

		/// <summary>
		/// Setups the Initial Loading state
		/// </summary>
		public void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var final = stateFactory.Final("Final");
			var dataLoading = stateFactory.TaskWait("Initial device data loading");
			var uiLoading = stateFactory.TaskWait("Initial Ui loading");
			var compliance = stateFactory.State("Compliance Pop Up");
			var complianceCheck = stateFactory.Choice("Compliance Check");
			
			initial.Transition().Target(dataLoading);
			initial.OnExit(SubscribeEvents);
			
			dataLoading.OnEnter(InitPlugins);
			dataLoading.WaitingFor(LoadConfigs).Target(uiLoading);
			dataLoading.OnExit(InitGameLogic);
			
			uiLoading.WaitingFor(LoadInitialUi).Target(complianceCheck);
			
			complianceCheck.Transition().Condition(IsComplianceAccepted).Target(final);
			complianceCheck.Transition().Target(compliance);
			
			compliance.OnEnter(OpenCompliancePopUp);
			compliance.Event(_compliance_Clicked_Event).Target(final);
			compliance.OnExit(CloseCompliancePopUp);
			
			final.OnEnter(FirstTimeSetup);
			final.OnEnter(UnsubscribeEvents);
		}

		private void SubscribeEvents()
		{
			_services.MessageBrokerService.Subscribe<ApplicationComplianceAcceptedMessage>(OnApplicationComplianceAcceptedMessage);
		}

		private void UnsubscribeEvents()
		{
			_services.MessageBrokerService.UnsubscribeAll(this);
		}

		private void InitPlugins()
		{
			if (Debug.isDebugBuild)
			{
				//SRDebug.Init();
			}
		}

		private async UniTask LoadInitialUi()
		{
			await UniTask.WhenAll(_uiService.LoadUiSetAsync((int)UiSetId.InitialLoadUi));
		}

		private void InitGameLogic()
		{
			LoadGameData();
			_gameLogic.Init(_dataService, _services);
		}

		private void FirstTimeSetup()
		{
			_services.CommandService.ExecuteCommand(new SetupFirstTimePlayerCommand());
		}

		private bool IsComplianceAccepted()
		{
			return _gameLogic.AppLogic.IsComplianceAccepted || !Constants.Settings.IsComplianceEnabled;
		}

		private async UniTask LoadConfigs()
		{
			var tasks = new UniTask[]
			{ 
				// Custome Configs
				_services.AssetResolverService.LoadAssetAsync<UiConfigs>(
					AddressableId.Addressables_Configs_UiConfigs.GetConfig().Address,
					result => _uiService.Init(result)),

				// Singleton Configs
				_services.AssetResolverService.LoadAssetAsync<GameConfigs>(
					AddressableId.Addressables_Configs_GameConfigs.GetConfig().Address,
					result => _configsAdder.AddSingletonConfig(result.Config)),

				// Collection Configs
				_services.AssetResolverService.LoadAssetAsync<DataConfigs>(
					AddressableId.Addressables_Configs_DataConfigs.GetConfig().Address,
					result => _configsAdder.AddConfigs(data => (int)data.Id, result.Configs)),

				// Assets Configs
				_services.AssetResolverService.LoadAssetAsync<SceneAssetConfigs>(
					AddressableId.Addressables_Configs_SceneAssetConfigs.GetConfig().Address,
					result => _assetAdderService.AddConfigs(result))
			};

			await UniTask.WhenAll(tasks);

			// TODO: Find a solution to unload all the configs as data was already added
		}

		private void LoadGameData()
		{
			var time = _services.TimeService.DateTimeUtcNow;
			var appData = _dataService.LoadData<AppData>();
			var rngData = _dataService.LoadData<RngData>();
			var playerData = _dataService.LoadData<PlayerData>();

			// First time opens the app
			if (appData.SessionCount == 0)
			{
				var seed = (int)(time.Ticks & int.MaxValue);

				appData.FirstLoginTime = time;
				appData.LoginTime = time;
				rngData.Seed = seed;
				rngData.State = RngService.GenerateRngState(seed);
			}

			appData.SessionCount += 1;
			appData.LastLoginTime = appData.LoginTime;
			appData.LoginTime = time;
		}

		private void OnApplicationComplianceAcceptedMessage(ApplicationComplianceAcceptedMessage obj)
		{
			_statechartTrigger(_compliance_Clicked_Event);
		}

		private void OpenCompliancePopUp()
		{
			_uiService.OpenUiAsync<ComplianceScreenPresenter>().Forget();
		}
		
		private void CloseCompliancePopUp()
		{
			_uiService.CloseUi<ComplianceScreenPresenter>(true);
		}
	}
}