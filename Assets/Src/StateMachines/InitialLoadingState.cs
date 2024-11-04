using System.Threading.Tasks;
using Game.Configs;
using Game.Data;
using GameLovers.ConfigsProvider;
using GameLovers.Services;
using GameLovers.StatechartMachine;
using GameLovers.UiService;
using Game.Ids;
using Game.Logic;
using Newtonsoft.Json;
using Game.Services;
using UnityEngine;
using Game.Commands;

namespace Game.StateMachines
{
	/// <summary>
	/// This class represents the Loading state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	internal class InitialLoadingState
	{
		private readonly IGameServices _services;
		private readonly IGameLogicInit _gameLogic;
		private readonly IGameUiServiceInit _uiService;
		private readonly IConfigsAdder _configsAdder;
		private readonly IDataService _dataService;

		public InitialLoadingState(IInstaller installer)
		{
			_gameLogic = installer.Resolve<IGameLogicInit>();
			_services = installer.Resolve<IGameServices>();
			_uiService = installer.Resolve<IGameUiServiceInit>();
			_configsAdder = installer.Resolve<IConfigsAdder>();
			_dataService = installer.Resolve<IDataService>();
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
			
			initial.Transition().Target(dataLoading);
			initial.OnExit(SubscribeEvents);
			
			dataLoading.OnEnter(InitPlugins);
			dataLoading.OnEnter(LoadGameData);
			dataLoading.WaitingFor(LoadConfigs).Target(uiLoading);
			dataLoading.OnExit(InitGameLogic);
			
			uiLoading.WaitingFor(LoadInitialUi).Target(final);
			
			final.OnEnter(UnsubscribeEvents);
		}

		private void UnsubscribeEvents()
		{
			_services.MessageBrokerService.UnsubscribeAll(this);
		}

		private void SubscribeEvents()
		{
			// Add any events to subscribe
		}

		private void InitPlugins()
		{
			if (Debug.isDebugBuild)
			{
				//SRDebug.Init();
			}
		}

		private async Task LoadInitialUi()
		{
			await Task.WhenAll(_uiService.LoadUiSetAsync((int) UiSetId.InitialLoadUi));
		}

		private void InitGameLogic()
		{
			LoadGameData();
			_gameLogic.Init(_dataService, _services);
			_services.CommandService.ExecuteCommand(new SetupFirstTimePlayerCommand());
		}

		private async Task LoadConfigs()
		{
			var uiConfigs = await _services.AssetResolverService.LoadAssetAsync<UiConfigs>(AddressableId.Prefabs_Configs_UiConfigs.GetConfig().Address);
			var gameConfigs = await _services.AssetResolverService.LoadAssetAsync<GameConfigs>(AddressableId.Prefabs_Configs_GameConfigs.GetConfig().Address);
			var dataConfigs = await _services.AssetResolverService.LoadAssetAsync<DataConfigs>(AddressableId.Prefabs_Configs_DataConfigs.GetConfig().Address);
			
			_uiService.Init(uiConfigs);
			_configsAdder.AddSingletonConfig(gameConfigs.Config);
			_configsAdder.AddConfigs(data => (int) data.Id, dataConfigs.Configs);
			
			_services.AssetResolverService.UnloadAsset(uiConfigs);
			_services.AssetResolverService.UnloadAsset(gameConfigs);
			_services.AssetResolverService.UnloadAsset(dataConfigs);
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
	}
}