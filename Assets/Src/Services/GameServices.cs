using GameLovers.ConfigsProvider;
using GameLovers.Services;
using Game.Logic;

namespace Game.Services
{
	/// <summary>
	/// Use this contract in order to have a standardized Init process stream within the Service being implemented
	/// </summary>
	public interface IServiceInit
	{
		/// <summary>
		/// Initializes the service values and functions to properly execute
		/// </summary>
		void Init(IGameServices services);
	}

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

		/// <inheritdoc cref="IConfigsProvider"/>
		IConfigsProvider ConfigsProvider { get; }
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
		/// <inheritdoc cref="IAnalyticsService"/>
		IAnalyticsService AnalyticsService { get; }
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
		public IAssetResolverService AssetResolverService { get; }
		/// <inheritdoc />
		public IWorldObjectReferenceService WorldObjectReferenceService { get; }
		/// <inheritdoc />
		public IConfigsProvider ConfigsProvider { get; }
		/// <inheritdoc />
		public IAnalyticsService AnalyticsService { get; }

		public GameServices(IInstaller installer)
		{
			MessageBrokerService = installer.Resolve<IMessageBrokerService>();
			TimeService = installer.Resolve<ITimeService>();
			ConfigsProvider = installer.Resolve<IConfigsProvider>();
			AssetResolverService = installer.Resolve<IAssetResolverService>();
			AnalyticsService = installer.Resolve<IAnalyticsService>();
			PoolService = installer.Resolve<IPoolService>();
			TickService = installer.Resolve<ITickService>();
			CoroutineService = installer.Resolve<ICoroutineService>();
		}

		/// <summary>
		/// <inheritdoc cref="IServiceInit.Init(IGameServices)"/>
		/// </summary>
		public void Init()
		{
			// Init any needed service
		}
	}
}