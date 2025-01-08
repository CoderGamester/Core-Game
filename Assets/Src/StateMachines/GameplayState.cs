using System;
using GameLovers.Services;
using GameLovers.StatechartMachine;
using Game.Ids;
using Game.Services;
using UnityEngine;
using Game.Presenters;
using Game.Messages;
using Game.Commands;
using Game.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Game.Utils;

namespace Game.StateMachines
{
	/// <summary>
	/// This object contains the behaviour logic for the Gameplay State in the <seealso cref="GameStateMachine"/>
	/// </summary>
	public class GameplayState
	{
		public static readonly IStatechartEvent Game_Over_Event = new StatechartEvent("Game Over Event");
		public static readonly IStatechartEvent Game_Restart_Event = new StatechartEvent("Game Restart Event");
		
		private static readonly IStatechartEvent _pause_Clicked_Event = new StatechartEvent("Pause Clicked Event");
		private static readonly IStatechartEvent _menu_Clicked_Event = new StatechartEvent("Menu Clicked Event");
		private static readonly IStatechartEvent _close_Clicked_Event = new StatechartEvent("Close Clicked Event");

		private readonly IGameUiService _uiService;
		private readonly IGameServicesLocator _services;
		private readonly IGameDataProviderLocator _gameDataProvider;
		private readonly Action<IStatechartEvent> _statechartTrigger;

		public GameplayState(IInstaller installer, Action<IStatechartEvent> statechartTrigger)
		{
			_gameDataProvider = installer.Resolve<IGameDataProviderLocator>();
			_services = installer.Resolve<IGameServicesLocator>();
			_uiService = installer.Resolve<IGameUiServiceInit>();
			_statechartTrigger = statechartTrigger;
		}

		/// <summary>
		/// Setups the Adventure gameplay state
		/// </summary>
		public void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var final = stateFactory.Final("Final");
			var gameplayLoading = stateFactory.TaskWait("Gameplay Loading");
			var gameStateCheck = stateFactory.Choice("GameOver Check");
			var gameplay = stateFactory.State("Gameplay");
			var gameOver = stateFactory.State("GameOver");
			var pauseScreen = stateFactory.State("Pause Screen");

			initial.Transition().Target(gameplayLoading);
			initial.OnExit(SubscribeEvents);

			gameplayLoading.WaitingFor(LoadGameplayAssets).Target(gameStateCheck);

			gameStateCheck.OnEnter(GameInit);
			gameStateCheck.Transition().Condition(IsGameOver).Target(gameOver);
			gameStateCheck.Transition().Target(gameplay);

			gameplay.OnEnter(OpenGameplayUi);
			gameplay.Event(Game_Over_Event).Target(gameOver);
			gameplay.Event(_pause_Clicked_Event).Target(pauseScreen);
			gameplay.OnExit(CloseGameplayUi);
			
			pauseScreen.OnEnter(OpenPauseScreenUi);
			pauseScreen.Event(Game_Over_Event).Target(gameOver);
			pauseScreen.Event(Game_Restart_Event).Target(gameStateCheck);
			pauseScreen.Event(_close_Clicked_Event).Target(gameplay);
			pauseScreen.Event(_menu_Clicked_Event).Target(final);
			pauseScreen.OnExit(ClosePauseScreenUi);

			gameOver.OnEnter(OpenGameOverUi);
			gameOver.Event(Game_Restart_Event).Target(gameStateCheck);
			gameOver.OnExit(CloseGameOverUi);

			final.OnEnter(UnloadAssets);
			final.OnEnter(UnsubscribeEvents);
		}

		private void SubscribeEvents()
		{
			_services.MessageBrokerService.Subscribe<OnGameOverMessage>(OnGameOverMessage);
			_services.MessageBrokerService.Subscribe<OnGameRestartMessage>(OnGameRestartMessage);
		}

		private void UnsubscribeEvents()
		{
			_services.MessageBrokerService.UnsubscribeAll(this);
		}

		private void OnGameOverMessage(OnGameOverMessage message)
		{
			_statechartTrigger(Game_Over_Event);
		}

		private void OnGameRestartMessage(OnGameRestartMessage message)
		{
			_statechartTrigger(Game_Restart_Event);
		}

		private void GameInit()
		{
			_services.MessageBrokerService.Publish(new OnGameInitMessage());
		}

		private bool IsGameOver()
		{
			return false;
		}

		private void OpenPauseScreenUi()
		{
			var data = new PausePopUpPresenter.PresenterData
			{
				OnReturnMenuClicked = () => _statechartTrigger(_menu_Clicked_Event),
				OnCloseClicked = () => _statechartTrigger(_close_Clicked_Event)
			};
			
			_uiService.OpenUiAsync<PausePopUpPresenter, PausePopUpPresenter.PresenterData>(data).Forget();
		}

		private void ClosePauseScreenUi()
		{
			_uiService.CloseUi<PausePopUpPresenter>();
		}

		private void OpenGameplayUi()
		{
			var data = new MainHudPresenter.PresenterData
			{
				OnPauseClicked = () => _statechartTrigger(_pause_Clicked_Event)
			};
			
			_uiService.OpenUiAsync<MainHudPresenter, MainHudPresenter.PresenterData>(data).Forget();
		}

		private void CloseGameplayUi()
		{
			_uiService.CloseUi<MainHudPresenter>();
		}

		private void OpenGameOverUi()
		{
			_uiService.OpenUiAsync<GameOverScreenPresenter>().Forget();
		}

		private void CloseGameOverUi()
		{
			_uiService.CloseUi<GameOverScreenPresenter>();
		}

		private async UniTask LoadGameplayAssets()
		{
			await UniTask.WhenAll(
				_uiService.LoadGameUiSet(UiSetId.GameplayUi, 0.8f),
				_services.AssetResolverService.LoadSceneAsync(SceneId.Game, LoadSceneMode.Additive));
		}

		private void UnloadAssets()
		{
			_uiService.UnloadGameUiSet(UiSetId.GameplayUi);
			_services.AssetResolverService.UnloadSceneAsync(SceneId.Game).Forget();
			Resources.UnloadUnusedAssets();
		}
	}
}