using GameLovers.GoogleSheetImporter;
using GameLovers.Services;
using GameLovers.Statechart;
using Logic;
using Services;
using UnityEngine;

namespace StateMachines
{
	/// <summary>
	/// The State Machine that controls the entire flow of the game
	/// </summary>
	public class GameStateMachine
	{
		private readonly IStateMachine _stateMachine;
		private readonly IGameServices _services;
		private readonly IGameUiServiceInit _uiService;
		private readonly InitialLoadingState _initialLoadingState;
		private readonly GameplayState _gameplayState;

		/// <inheritdoc cref="IStateMachine.LogsEnabled"/>
		public bool LogsEnabled
		{
			get => _stateMachine.LogsEnabled;
			set => _stateMachine.LogsEnabled = value;
		}

		public GameStateMachine(IGameLogicInit gameLogic, IGameServices services, IGameUiServiceInit uiService, 
		                        IConfigsAdder configsAdder, IDataService dataService)
		{
			_services = services;
			_uiService = uiService;
			
			_initialLoadingState = new InitialLoadingState(gameLogic, _services, _uiService, configsAdder, dataService);
			_gameplayState = new GameplayState(_services, _uiService, Trigger);
			_stateMachine = new StateMachine(Setup);
		}

		/// <inheritdoc cref="IStatechart.LogsEnabled"/>
		public void Run()
		{
			_stateMachine.Run();
		}

		private void Trigger(IStatechartEvent eventTrigger)
		{
			_stateMachine.Trigger(eventTrigger);
		}

		private void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var final = stateFactory.Final("Final");
			var initialLoading = stateFactory.Nest("Initial Loading");
			var game = stateFactory.Nest("Game");
			
			initial.Transition().Target(initialLoading);
			initial.OnExit(SubscribeEvents);
			
			initialLoading.Nest(_initialLoadingState.Setup).Target(game);
			
			game.Nest(_gameplayState.Setup).Target(final);
			
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
	}
}