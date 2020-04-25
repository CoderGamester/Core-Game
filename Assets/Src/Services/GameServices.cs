using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Services
{
	/// <summary>
	/// Provides access to all game's common helper services
	/// This services are stateless interfaces that establishes a set of available operations with deterministic response
	/// without manipulating any game’s data
	/// </summary>
	/// <remarks>
	/// Follows the "Service Locator Pattern" <see cref="https://www.geeksforgeeks.org/service-locator-pattern/"/>
	/// </remarks>
	public interface IGameServices
	{
		/// <inheritdoc cref="IMessageBrokerService"/>
		IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc cref="ICommandService"/>
		ICommandService CommandService { get; }
		/// <inheritdoc cref="IPoolService"/>
		IPoolService PoolService { get; }
		/// <inheritdoc cref="IUiService"/>
		IUiService UiService { get; }
		/// <inheritdoc cref="ITickService"/>
		ITickService TickService { get; }
		/// <inheritdoc cref="ITimeService"/>
		ITimeService TimeService { get; }
		/// <inheritdoc cref="ICoroutineService"/>
		ICoroutineService CoroutineService { get; }
		/// <inheritdoc cref="IWorldObjectReferenceService"/>
		IWorldObjectReferenceService WorldObjectReferenceService { get; }
	}

	/// <inheritdoc />
	public class GameServices : IGameServices
	{
		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc />
		public ICommandService CommandService { get; }
		/// <inheritdoc />
		public IPoolService PoolService { get; }
		/// <inheritdoc />
		public IUiService UiService { get; }
		/// <inheritdoc />
		public ITickService TickService { get; }
		/// <inheritdoc />
		public ITimeService TimeService { get; }
		/// <inheritdoc />
		public ICoroutineService CoroutineService { get; }
		/// <inheritdoc />
		public IWorldObjectReferenceService WorldObjectReferenceService { get; }

		public GameServices(IMessageBrokerService messageBrokerService, ITimeService timeService, IGameLogic gameLogic,
			IWorldObjectReferenceService worldObjectReference)
		{
			MessageBrokerService = messageBrokerService;
			TimeService = timeService;
			WorldObjectReferenceService = worldObjectReference;
			
			CommandService = new CommandService(gameLogic);
			PoolService = new PoolService();
			UiService = new UiService();
			TickService =  new TickService();
			CoroutineService = new CoroutineService();
		}
	}
}