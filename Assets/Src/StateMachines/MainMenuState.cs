using Cysharp.Threading.Tasks;
using Game.Ids;
using Game.Logic;
using Game.Messages;
using Game.Presenters;
using Game.Services;
using Game.Services.Analytics;
using GameLovers.Services;
using GameLovers.StatechartMachine;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.StateMachines
{
	/// <summary>
	/// This class represents the Main menu state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	public class MainMenuState
	{
		private static readonly IStatechartEvent _play_Clicked_Event = new StatechartEvent("Play Button Clicked Event");

		private readonly IGameUiService _uiService;
		private readonly IGameServicesLocator _services;
		private readonly IGameDataProviderLocator _gameDataProvider;
		private readonly Action<IStatechartEvent> _statechartTrigger;

		public MainMenuState(IInstaller installer, Action<IStatechartEvent> statechartTrigger)
		{
			_uiService = installer.Resolve<IGameUiService>();
			_services = installer.Resolve<IGameServicesLocator>();
			_gameDataProvider = installer.Resolve<IGameDataProviderLocator>();
			_statechartTrigger = statechartTrigger;
		}

		public void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var final = stateFactory.Final("Final");
			var menuLoading = stateFactory.TaskWait("Menu Loading");
			var mainScreen = stateFactory.State("Main Screen");

			initial.Transition().Target(menuLoading);
			initial.OnExit(SubscribeEvents);

			menuLoading.OnEnter(MenuLoadingStart);
			menuLoading.WaitingFor(LoadMenuAssets).Target(mainScreen);
			menuLoading.OnExit(MenuLoadingEnd);

			mainScreen.OnEnter(OpenMainScreenUi);
			mainScreen.Event(_play_Clicked_Event).Target(final);
			mainScreen.OnExit(CloseMainScreenUi);

			final.OnEnter(UnloadAssets);
			final.OnEnter(UnsubscribeEvents);
		}

		private void SubscribeEvents()
		{
			// Subscribe to any events
		}

		private void UnsubscribeEvents()
		{
			_services.MessageBrokerService.UnsubscribeAll(this);
		}

		private void OpenMainScreenUi()
		{
			var data = new MainMenuPresenter.PresenterData
			{
				OnPlayButtonClicked = () => _statechartTrigger(_play_Clicked_Event)
			};
			
			_services.AnalyticsService.MainMenuCalls.MainMenuEnter();
			_uiService.OpenUiAsync<MainMenuPresenter, MainMenuPresenter.PresenterData>(data).Forget();
		}

		private void CloseMainScreenUi()
		{
			_uiService.CloseUi<MainMenuPresenter>();
		}

		private async UniTask LoadMenuAssets()
		{
			await UniTask.WhenAll(
				_uiService.LoadGameUiSet(UiSetId.MenuUi, 0.8f),
				_services.AssetResolverService.LoadSceneAsync(SceneId.Menu, LoadSceneMode.Additive));
		}

		private void UnloadAssets()
		{
			_uiService.UnloadGameUiSet(UiSetId.MenuUi);
			_services.AssetResolverService.UnloadSceneAsync(SceneId.Menu).Forget();
			Resources.UnloadUnusedAssets();
		}

		private void MenuLoadingStart()
		{
			_services.AnalyticsService.SessionCalls.LoadingCompleted(SceneId.Menu.ToString());
		}

		private void MenuLoadingEnd()
		{
			_services.AnalyticsService.SessionCalls.LoadingStarted(SceneId.Menu.ToString());
		}
	}
}
