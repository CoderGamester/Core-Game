using GameLovers.GoogleSheetImporter;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Logic;
using Services;
using UnityEngine;

namespace StateMachines
{
	/// <summary>
	/// The State Machine that controls the entire flow of the game
	/// </summary>
	public class GameStateMachine : IStatechart
	{
		private readonly IStatechart _stateMachine;
		private readonly IGameLogicInit _gameLogic;
		private readonly IGameServices _services;
		private readonly IUiService _uiService;
		private readonly LoadingState _loadingState;

		/// <inheritdoc />
		public bool LogsEnabled
		{
			get => _stateMachine.LogsEnabled;
			set => _stateMachine.LogsEnabled = value;
		}

		public GameStateMachine(IGameLogicInit gameLogic, IGameServices services, IUiService uiService, IConfigsAdder configsAdder)
		{
			_gameLogic = gameLogic;
			_services = services;
			_uiService = uiService;
			
			_loadingState = new LoadingState(configsAdder, _services, _uiService);
			_stateMachine = new Statechart(Setup);
		}

		/// <inheritdoc />
		public void Trigger(IStatechartEvent trigger)
		{
			_stateMachine.Trigger(trigger);
		}

		/// <inheritdoc />
		public void Run()
		{
			_stateMachine.Run();
		}

		/// <inheritdoc />
		public void Pause()
		{
			_stateMachine.Pause();
		}

		/// <inheritdoc />
		public void Reset()
		{
			_stateMachine.Reset();
		}

		private void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var initialLoading = stateFactory.Wait("Initial Loading");
			var finalLoading = stateFactory.Wait("Final Loading");
			var game = stateFactory.State("Game");
			
			initial.Transition().Target(initialLoading);
			
			initialLoading.OnEnter(InitPlugins);
			initialLoading.WaitingFor(_loadingState.InitialLoading).Target(finalLoading);
			
			finalLoading.OnEnter(_gameLogic.Init);
			finalLoading.WaitingFor(_loadingState.FinalLoading).Target(game);
			
			game.OnEnter(OpenGameUi);
		}

		private void InitPlugins()
		{
			if (Debug.isDebugBuild)
			{
				//SRDebug.Init();
			}
		}

		private void OpenGameUi()
		{
			_uiService.OpenUiSet((int) UiSetId.MainUi, false);
		}
	}
}