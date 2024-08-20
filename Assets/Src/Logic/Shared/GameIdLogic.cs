using Game.Data;
using GameLovers;
using GameLovers.ConfigsProvider;
using GameLovers.Services;
using Game.Ids;

namespace Game.Logic.Shared
{
	/// <summary>
	/// This logic provides the necessary behaviour to manage any entity <seealso cref="GameId"/> relationship with an unique <seealso cref="UniqueId"/>
	/// </summary>
	public interface IGameIdDataProvider
	{
		/// <summary>
		/// Requests the <see cref="IObservableDictionary{TKey,TValue}"/> representation in readonly form of the <see cref="GameId"/> data
		/// </summary>
		IObservableDictionaryReader<UniqueId, GameId> Ids { get; }

		/// <summary>
		/// Requests the <see cref="UniqueId"/> for the first element found with the given <paramref name="gameId"/>
		/// </summary>
		/// <exception cref="LogicException">
		/// Thrown if there is no element with the given <paramref name="gameId"/>
		/// </exception>
		UniqueId GetUniqueId(GameId gameId);

		/// <summary>
		/// Requests the <see cref="UniqueId"/> for the first element found with the given <paramref name="gameId"/>
		/// Returns true if the element was found
		/// </summary>
		bool TryGetUniqueId(GameId gameId, out UniqueId data);
	}

	/// <inheritdoc />
	public interface IGameIdLogic : IGameIdDataProvider
	{
		/// <summary>
		/// Requests the <see cref="IObservableDictionary{TKey,TValue}"/> representation of the <see cref="GameId"/> data
		/// </summary>
		new IObservableDictionary<UniqueId, GameId> Ids { get; }
	}
	
	/// <inheritdoc cref="IGameIdLogic" />
	public class GameIdLogic : AbstractBaseLogic<PlayerData>, IGameIdLogic, IGameLogicInitializer
	{
		/// <inheritdoc />
		IObservableDictionaryReader<UniqueId, GameId> IGameIdDataProvider.Ids => Ids;
		/// <inheritdoc />
		public IObservableDictionary<UniqueId, GameId> Ids { get; private set; }

		public GameIdLogic(IConfigsProvider configsProvider, IDataService dataService, ITimeService timeService) :
			base(configsProvider, dataService, timeService)
		{
		}

		/// <inheritdoc />
		public void Init()
		{
			Ids = new ObservableDictionary<UniqueId, GameId>(Data.GameIds);
		}

		/// <inheritdoc />
		public UniqueId GetUniqueId(GameId gameId)
		{
			if (TryGetUniqueId(gameId, out var data))
			{
				return data;
			}

			throw new LogicException($"There is no data for the {gameId}");
		}

		/// <inheritdoc />
		public bool TryGetUniqueId(GameId gameId, out UniqueId data)
		{
			foreach (var pair in Ids)
			{
				if (pair.Value == gameId)
				{
					data = pair.Key;

					return true;
				}
			}
			
			data = UniqueId.Invalid;
			
			return false;
		}
	}
}