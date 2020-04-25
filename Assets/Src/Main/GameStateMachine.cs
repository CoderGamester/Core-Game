using GameLovers.ConfigsContainer;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Logic;
using Services;

namespace Main
{
	/// <summary>
	/// The State Machine that controls the entire flow of the game
	/// </summary>
	public class GameStateMachine : IStatechart
	{
		private readonly IStatechart _stateMachine;
		private readonly IGameInternalLogic _gameLogic;
		private readonly IGameServices _services;
		private readonly LoadingState _loadingState;

		/// <inheritdoc />
		public bool LogsEnabled
		{
			get => _stateMachine.LogsEnabled;
			set => _stateMachine.LogsEnabled = value;
		}

		public GameStateMachine(IGameInternalLogic gameLogic, IGameServices services)
		{
			var configsProvider = (ConfigsProvider) gameLogic.ConfigsProvider;
			var uiService = (UiService) services.UiService;
			
			_gameLogic = gameLogic;
			_services = services;
			_loadingState = new LoadingState(configsProvider, uiService);
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
			
			initialLoading.WaitingFor(_loadingState.InitialLoading).Target(finalLoading);
			
			finalLoading.OnEnter(_gameLogic.Init);
			finalLoading.WaitingFor(_loadingState.FinalLoading).Target(game);
			
			game.OnEnter(StartGame);
		}

		private void StartGame()
		{
			_services.UiService.OpenUiSet((int) UiSetId.MainUi, false);
		}
	}
}