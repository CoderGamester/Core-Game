using GameLovers.Services;
using GameLovers.UiService;
using Logic;

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
		/// <inheritdoc cref="IDataSaver"/>
		IDataSaver DataSaver { get; }
		
		/// <inheritdoc cref="IMessageBrokerService"/>
		IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc cref="ICommandService{T}"/>
		ICommandService<IGameLogic> CommandService { get; }
		/// <inheritdoc cref="IPoolService"/>
		IPoolService PoolService { get; }
		/// <inheritdoc cref="ITickService"/>
		ITickService TickService { get; }
		/// <inheritdoc cref="ITimeService"/>
		ITimeService TimeService { get; }
		/// <inheritdoc cref="ICoroutineService"/>
		ICoroutineService CoroutineService { get; }
		/// <inheritdoc cref="INetworkService"/>
		INetworkService NetworkService { get; }
		/// <inheritdoc cref="IAssetResolverService"/>
		IAssetResolverService AssetResolverService { get; }
		/// <inheritdoc cref="IWorldObjectReferenceService"/>
		IWorldObjectReferenceService WorldObjectReferenceService { get; }
	}

	/// <inheritdoc />
	public class GameServices : IGameServices
	{
		/// <inheritdoc />
		public IDataSaver DataSaver { get; }

		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc />
		public ICommandService<IGameLogic> CommandService { get; }
		/// <inheritdoc />
		public IPoolService PoolService { get; }
		/// <inheritdoc />
		public ITickService TickService { get; }
		/// <inheritdoc />
		public ITimeService TimeService { get; }
		/// <inheritdoc />
		public ICoroutineService CoroutineService { get; }
		/// <inheritdoc />
		public INetworkService NetworkService { get; }
		/// <inheritdoc />
		public IAssetResolverService AssetResolverService { get; }

		/// <inheritdoc />
		public IWorldObjectReferenceService WorldObjectReferenceService { get; }

		public GameServices(IMessageBrokerService messageBrokerService, ITimeService timeService, IDataSaver dataSaver,
		                    IGameLogic gameLogic, IWorldObjectReferenceService worldObjectReference)
		{
			var networkService = new GameNetworkService();

			NetworkService = networkService;
			MessageBrokerService = messageBrokerService;
			TimeService = timeService;
			WorldObjectReferenceService = worldObjectReference;
			DataSaver = dataSaver;
			
			CommandService = new CommandService<IGameLogic>(gameLogic, networkService);
			PoolService = new PoolService();
			AssetResolverService = new AssetResolverService();
			TickService =  new TickService();
			CoroutineService = new CoroutineService();
		}
	}
}