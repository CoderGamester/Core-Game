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
		
		public InitialLoadingState(IGameLogicInit gameLogic, IGameServices services, IInstaller installer)
		{
			_gameLogic = gameLogic;
			_services = services;
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
			dataLoading.OnExit(_gameLogic.Init);
			
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

		private async Task LoadConfigs()
		{
			var uiConfigs = await _services.AssetResolverService.LoadAssetAsync<UiConfigs>(AddressableId.Addressables_Configs_UiConfigs.GetConfig().Address);
			var gameConfigs = await _services.AssetResolverService.LoadAssetAsync<GameConfigs>(AddressableId.Addressables_Configs_GameConfigs.GetConfig().Address);
			var dataConfigs = await _services.AssetResolverService.LoadAssetAsync<DataConfigs>(AddressableId.Addressables_Configs_DataConfigs.GetConfig().Address);
			
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
			var appDataJson = PlayerPrefs.GetString(nameof(AppData), "");
			var playerDataJson = PlayerPrefs.GetString(nameof(PlayerData), "");
			var appData = string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson);
			var playerData = string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

			if (string.IsNullOrEmpty(appDataJson))
			{
				appData.FirstLoginTime = time;
				appData.LoginTime = time;
			}
			
			appData.LastLoginTime = appData.LoginTime;
			appData.LoginTime = time;
			
			_dataService.AddOrReplaceData(appData);
			_dataService.AddOrReplaceData(playerData);
		}
	}
}