using Game.Data;
using Game.Ids;
using GameLovers.ConfigsProvider;
using GameLovers.Services;

namespace Game.Logic.Shared
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
		private IGameLogic _gameLogic;

		/// <inheritdoc />
		public UniqueId LastUniqueId => Data.UniqueIdCounter;

		public EntityFactoryLogic(
			IGameLogic gamelogic,
			IConfigsProvider configsProvider,
			IDataProvider dataProvider,
			ITimeService timeService) :
			base(configsProvider, dataProvider, timeService)
		{
			_gameLogic = gamelogic;
		}
	}
}
