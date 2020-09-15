using System.Threading.Tasks;
using Configs;
using GameLovers.GoogleSheetImporter;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Presenters;
using Services;
using UnityEngine;

namespace StateMachines
{
	/// <summary>
	/// This class represents the Loading state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	internal class LoadingState
	{
		private readonly IConfigsAdder _configsAdder;
		private readonly IGameServices _services;
		private readonly IUiService _uiService;
		
		public LoadingState(IConfigsAdder configsAdder, IGameServices services, IUiService uiService)
		{
			_configsAdder = configsAdder;
			_services = services;
			_uiService = uiService;
		}

		/// <summary>
		/// Executes the initial Loading process when the game starts with the given <paramref name="activity"/> to be
		/// invoked when this loading step is complete.
		/// It loads and initializes the basic game configurations and assets.
		/// </summary>
		public async void InitialLoading(IWaitActivity activity)
		{
			await LoadUiConfigsConfigs();
			await LoadOpenLoadingScreen();
			await LoadConfigs(0.2f);
			await LoadInitialUis(0.6f);
			
			activity.Complete();
		}

		/// <summary>
		/// Executes the final Loading process when the game starts with the given <paramref name="activity"/> to be
		/// invoked when this loading step is complete.
		/// It loads and initializes the game world and make it ready to start
		/// </summary>
		public async void FinalLoading(IWaitActivity activity)
		{
			await LoadMaiUi(0.8f);
			await LoadGameWorld(1f);

			Resources.UnloadUnusedAssets();

			// Small delay to give the loading complete feedback
			await Task.Delay(500);

			_uiService.CloseUi<LoadingScreenPresenter>();
			activity.Complete();
		}

		private async Task LoadUiConfigsConfigs()
		{
			var configs = await _services.AssetResolverService.LoadAssetAsync<UiConfigs>(AddressableId.Configs_UiConfigs.GetConfig().Address);
			var uiService = (UiService) _uiService; // Ugly conversion but only necessary this one time for this purpose
			
			uiService.Init(configs);
			
			_services.AssetResolverService.UnloadAsset(configs);
		}

		private async Task LoadOpenLoadingScreen()
		{
			await _uiService.LoadUiAsync<LoadingScreenPresenter>();
			
			_uiService.OpenUi<LoadingScreenPresenter>().SetLoadingPercentage(0);
		}

		private async Task LoadConfigs(float loadingCap)
		{
			var gameConfigs = await _services.AssetResolverService.LoadAssetAsync<GameConfigs>(AddressableId.Configs_GameConfigs.GetConfig().Address);
			var dataConfigs = await _services.AssetResolverService.LoadAssetAsync<DataConfigs>(AddressableId.Configs_DataConfigs.GetConfig().Address);
			
			_configsAdder.AddSingletonConfig(gameConfigs.Config);
			_configsAdder.AddConfigs(data => (int) data.Id, dataConfigs.Configs);
			
			_services.AssetResolverService.UnloadAsset(gameConfigs);
			_services.AssetResolverService.UnloadAsset(dataConfigs);
			
			_uiService.GetUi<LoadingScreenPresenter>().SetLoadingPercentage(loadingCap);
		}

		private async Task LoadInitialUis(float loadingCap)
		{
			var loadingScreen = _uiService.GetUi<LoadingScreenPresenter>();
			var tasks = _uiService.LoadUiSetAsync((int) UiSetId.InitialLoadUi);
			var initialLoadingPercentage = loadingScreen.LoadingPercentage;
			var loadingBuffer = tasks.Length / loadingCap - initialLoadingPercentage;
			var loadedUiCount = 0f;

			// Load all initial uis
			foreach (var taskTemplate in tasks)
			{
				var task = await taskTemplate;
				var ui = await task;

				loadedUiCount++;

				loadingScreen.SetLoadingPercentage(initialLoadingPercentage + loadedUiCount / loadingBuffer);
				ui.gameObject.SetActive(false);
			}

			loadingScreen.SetLoadingPercentage(loadingCap);
		}

		private async Task LoadMaiUi(float loadingCap)
		{
			var loadingScreen = _uiService.GetUi<LoadingScreenPresenter>();
			var tasks = _uiService.LoadUiSetAsync((int) UiSetId.MainUi);
			var initialLoadingPercentage = loadingScreen.LoadingPercentage;
			var loadingBuffer = tasks.Length / loadingCap - initialLoadingPercentage;
			var loadedUiCount = 0f;

			// Load all initial uis
			foreach (var taskTemplate in tasks)
			{
				var task = await taskTemplate;
				var ui = await task;

				loadedUiCount++;

				loadingScreen.SetLoadingPercentage(initialLoadingPercentage + loadedUiCount / loadingBuffer);
				ui.gameObject.SetActive(false);
			}

			loadingScreen.SetLoadingPercentage(loadingCap);
		}

		private async Task LoadGameWorld(float loadingCap)
		{
			var loadingScreen = _uiService.GetUi<LoadingScreenPresenter>();

			// Load the Game World -> await AssetLoaderService.LoadSceneAsync(AddressableId.GameWorld.GetConfig().Address, LoadSceneMode.Additive);
			
			loadingScreen.SetLoadingPercentage(loadingCap);
		}
	}
}