using GameLovers.Services;
using GameLovers.StatechartMachine;
using Game.Logic;
using Game.Services;
using System;

namespace Game.StateMachines
{
	/// <summary>
	/// The State Machine that controls the entire flow of the game
	/// </summary>
	public class GameStateMachine : IDisposable
	{
		private readonly IStatechart _stateMachine;
		private readonly IGameServicesLocator _services;
		private readonly IGameUiServiceInit _uiService;
		private readonly InitialLoadingState _initialLoadingState;
		private readonly GameplayState _gameplayState;
		private readonly MainMenuState _mainMenuState;

		/// <inheritdoc cref="IStateMachine.LogsEnabled"/>
		public bool LogsEnabled
		{
			get => _stateMachine.LogsEnabled;
			set => _stateMachine.LogsEnabled = value;
		}

		public GameStateMachine(IInstaller installer)
		{
			_services = installer.Resolve<IGameServicesLocator>();
			_uiService = installer.Resolve<IGameUiServiceInit>();

			_initialLoadingState = new InitialLoadingState(installer);
			_gameplayState = new GameplayState(installer, Trigger);
			_mainMenuState = new MainMenuState(installer, Trigger);
			_stateMachine = new Statechart(Setup);
		}

		/// <inheritdoc cref="IStatechart.LogsEnabled"/>
		public void Run()
		{
			_stateMachine.Run();
		}

		/// <inheritdoc />
		public void Dispose()
		{
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
			var mainMenu = stateFactory.Nest("Main Menu");
			var game = stateFactory.Nest("Game");
			
			initial.Transition().Target(initialLoading);
			initial.OnExit(SubscribeEvents);
			
			initialLoading.Nest(_initialLoadingState.Setup).Target(mainMenu);

			mainMenu.Nest(_mainMenuState.Setup).Target(game);

			game.Nest(_gameplayState.Setup).Target(mainMenu);
			
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