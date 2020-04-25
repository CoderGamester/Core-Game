using Data;
using Ids;

namespace Logic
{
	/// <summary>
	/// This logic acts as a factory to create or remove all the entities in the game.
	/// It takes care that the right components are added and removed from any game's entities
	/// </summary>
	/// <remarks>
	/// Follows the "Factory Pattern" <see cref="https://en.wikipedia.org/wiki/Factory_method_pattern"/>
	/// </remarks>
	public interface IEntityDataProvider
	{
	}
	
	/// <inheritdoc />
	public interface IEntityLogic : IEntityDataProvider
	{
	}
	
	/// <inheritdoc />
	public class EntityLogic : IEntityLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDataProvider _dataProvider;
		
		private EntityLogic() {}

		public EntityLogic(IGameInternalLogic gameLogic, IDataProvider dataProvider)
		{
			_gameLogic = gameLogic;
			_dataProvider = dataProvider;
		}

		private void CreateGameIdData(UniqueId uniqueId, GameId gameId)
		{
			_dataProvider.PlayerData.GameIds.Add(uniqueId, gameId);
		}
	}
}