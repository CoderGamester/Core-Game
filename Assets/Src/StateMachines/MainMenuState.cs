using Cysharp.Threading.Tasks;
using Game.Ids;
using Game.Logic;
using Game.Messages;
using Game.Presenters;
using Game.Services;
using Game.Utils;
using GameLovers.Services;
using GameLovers.StatechartMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using static Game.Utils.Constants;

namespace Game.StateMachines
{
	/// <summary>
	/// This class represents the Main menu state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	public class MainMenuState
	{
		private static readonly IStatechartEvent PLAY_CLICKED_EVENT = new StatechartEvent("Play Button Clicked Event");

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

			menuLoading.WaitingFor(LoadMenuAssets).Target(mainScreen);

			mainScreen.OnEnter(OpenMainScreenUi);
			mainScreen.Event(PLAY_CLICKED_EVENT).Target(final);
			mainScreen.OnExit(CloseMainScreenUi);

			final.OnEnter(UnloadAssets);
			final.OnEnter(UnsubscribeEvents);
		}

		private void SubscribeEvents()
		{
			_services.MessageBrokerService.Subscribe<OnPlayClickedMessage>(OnPlayClickedMessage);
		}

		private void UnsubscribeEvents()
		{
			_services.MessageBrokerService.UnsubscribeAll(this);
		}

		private void OpenMainScreenUi()
		{
			_ = _uiService.OpenUiAsync<MainMenuPresenter>();
		}

		private void CloseMainScreenUi()
		{
			_uiService.CloseUi<MainMenuPresenter>();
		}

		private void OnPlayClickedMessage(OnPlayClickedMessage messagage)
		{
			_statechartTrigger(PLAY_CLICKED_EVENT);
		}

		private async UniTask LoadMenuAssets()
		{
			await UniTask.WhenAll(
				_uiService.LoadGameUiSet(UiSetId.MenuUi, 0.8f),
				_services.AssetResolverService.LoadSceneAsync(SceneId.Menu, LoadSceneMode.Additive));
			await Resources.UnloadUnusedAssets().ToUniTask();
		}

		private void UnloadAssets()
		{
			_services.AssetResolverService.UnloadSceneAsync(SceneId.Menu).Forget();
		}
	}
}
