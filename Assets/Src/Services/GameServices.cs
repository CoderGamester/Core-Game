using GameLovers.ConfigsProvider;
using GameLovers.Services;
using Game.Logic;
using UnityEditor;

namespace Game.Services
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
		//IWorldObjectReferenceService WorldObjectReferenceService { get; }
	}

	/// <inheritdoc />
	public class GameServices : IGameServices
	{
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
		//public IWorldObjectReferenceService WorldObjectReferenceService { get; }
		/// <inheritdoc />
		public IConfigsProvider ConfigsProvider { get; }
		/// <inheritdoc />
		public IAnalyticsService AnalyticsService { get; }

		public GameServices(IInstaller installer)
		{
			MessageBrokerService = installer.Resolve<IMessageBrokerService>();
			CommandService = installer.Resolve<ICommandService<IGameLogic>>();
			PoolService = installer.Resolve<IPoolService>();
			TickService = installer.Resolve<ITickService>();
			TimeService = installer.Resolve<ITimeService>();
			CoroutineService = installer.Resolve<ICoroutineService>();
			AssetResolverService = installer.Resolve<IAssetResolverService>();
			ConfigsProvider = installer.Resolve<IConfigsProvider>();
			AnalyticsService = installer.Resolve<IAnalyticsService>();
		}
	}
}