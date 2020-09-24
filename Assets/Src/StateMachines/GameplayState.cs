using System;
using System.Threading.Tasks;
using GameLovers.Statechart;
using Ids;
using Services;
using UnityEngine;

namespace StateMachines
{
	/// <summary>
	/// This object contains the behaviour logic for the Gameplay State in the <seealso cref="GameStateMachine"/>
	/// </summary>
	public class GameplayState
	{
		private readonly IGameUiService _uiService;
		private readonly IGameServices _services;
		private readonly Action<IStatechartEvent> _statechartTrigger;
		
		public GameplayState(IGameServices services, IGameUiService uiService, Action<IStatechartEvent> statechartTrigger)
		{
			_services = services;
			_uiService = uiService;
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
			var gameplay = stateFactory.State("Gameplay");
			
			initial.Transition().Target(gameplayLoading);
			initial.OnExit(SubscribeEvents);
			
			gameplayLoading.WaitingFor(LoadGameplay).Target(gameplay);
			
			gameplay.OnEnter(OpenGameplayUi);
			
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
		
		private async Task LoadGameplay()
		{
			await _uiService.LoadGameUiSet(UiSetId.GameplayUi, 0.8f);
			
			GC.Collect();
			Resources.UnloadUnusedAssets();
		}

		private void OpenGameplayUi()
		{
			_uiService.OpenUiSet((int) UiSetId.GameplayUi, false);
		}
	}
}