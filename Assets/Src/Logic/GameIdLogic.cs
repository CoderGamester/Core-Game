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
		/// Requests the Data dictionary in a readonly form
		/// </summary>
		IObservableDictionaryReader<UniqueId, GameId> Data { get; }

		/// <summary>
		/// Requests the <see cref="UniqueId"/> for the first element found with the given <paramref name="gameId"/>
		/// </summary>
		/// <exception cref="LogicException">
		/// Thrown if there is no element with the given <paramref name="gameId"/>
		/// </exception>
		UniqueId GetData(GameId gameId);

		/// <summary>
		/// Requests the <see cref="UniqueId"/> for the first element found with the given <paramref name="gameId"/>
		/// Returns true if the element was found
		/// </summary>
		bool TryGetData(GameId gameId, out UniqueId data);
	}

	/// <inheritdoc />
	public interface IGameIdLogic : IGameIdDataProvider
	{
	}
	
	/// <inheritdoc />
	public class GameIdLogic : IGameIdLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IObservableDictionary<UniqueId, GameId> _data;

		/// <inheritdoc />
		public IObservableDictionaryReader<UniqueId, GameId> Data => _data;

		private GameIdLogic() {}

		public GameIdLogic(IGameInternalLogic gameLogic, IObservableDictionary<UniqueId, GameId> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public UniqueId GetData(GameId gameId)
		{
			if (TryGetData(gameId, out var data))
			{
				return data;
			}

			throw new LogicException($"There is no data for the {gameId}");
		}

		/// <inheritdoc />
		public bool TryGetData(GameId gameId, out UniqueId data)
		{
			var dic = _data.GetDictionary();

			foreach (var pair in dic)
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