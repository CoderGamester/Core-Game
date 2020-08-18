using System;
using Data;
using GameLovers;
using Ids;
using TMPro;

namespace Logic
{
	/// <summary>
	/// This logic provides the necessary behaviour to manage any entity <seealso cref="GameId"/> relationship with an unique <seealso cref="UniqueId"/>
	/// </summary>
	public interface IGameIdDataProvider
	{
		/// <summary>
		/// Requests the <see cref="IObservableDictionary{TKey,TValue}"/> representation in readonly form of the <see cref="GameId"/> data
		/// </summary>
		IObservableDictionaryReader<UniqueId, GameId> Data { get; }

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
		new IObservableDictionary<UniqueId, GameId> Data { get; }
	}
	
	/// <inheritdoc />
	public class GameIdLogic : IGameIdLogic
	{
		private readonly IGameLogic _gameLogic;
		
		/// <inheritdoc />
		IObservableDictionaryReader<UniqueId, GameId> IGameIdDataProvider.Data => Data;
		/// <inheritdoc />
		public IObservableDictionary<UniqueId, GameId> Data { get; }

		private GameIdLogic() {}

		public GameIdLogic(IGameLogic gameLogic, PlayerData playerData)
		{
			_gameLogic = gameLogic;
			
			Data = new ObservableDictionary<UniqueId, GameId>(playerData.GameIds);
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
			foreach (var pair in Data)
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