using Game.Data;
using Game.Ids;
using GameLovers.ConfigsProvider;
using GameLovers.Services;

namespace Game.Logic.Server
{
	/// <summary>
	/// Provides the necessary behaviour to manage the creation of entities for the entire game logic
	/// </summary>
	public interface IEntityFactoryDataProvider
	{
		/// <summary>
		/// UniqueId of the last created entity
		/// </summary>
		UniqueId LastUniqueId { get; }
	}

	/// <inheritdoc />
	public interface IEntityFactoryLogic : IEntityFactoryDataProvider
	{
	}

	/// <inheritdoc cref="IEntityFactoryLogic"/>
	public class EntityFactoryLogic : AbstractBaseLogic<PlayerData>, IEntityFactoryLogic
	{
		private IGameLogicLocator _gameLogic;

		/// <inheritdoc />
		public UniqueId LastUniqueId => Data.UniqueIdCounter;

		public EntityFactoryLogic(
			IGameLogicLocator gamelogic,
			IConfigsProvider configsProvider,
			IDataProvider dataProvider,
			ITimeService timeService) :
			base(configsProvider, dataProvider, timeService)
		{
			_gameLogic = gamelogic;
		}
	}
}
