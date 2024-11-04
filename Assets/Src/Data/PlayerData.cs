using System;
using System.Collections.Generic;
using Game.Ids;

namespace Game.Data
{
	/// <summary>
	/// Contains all the data in the scope of the Player 
	/// </summary>
	[Serializable]
	public class PlayerData
	{
		public ulong UniqueIdCounter;

		public Dictionary<UniqueId, GameId> GameIds = new Dictionary<UniqueId, GameId>(new UniqueIdKeyComparer());
		public Dictionary<GameId, int> Currencies = new Dictionary<GameId, int>(new GameIdLookup.GameIdComparer())
		{
			{ GameId.SoftCurrency, 1000 },
			{ GameId.HardCurrency, 100 }
		};
	}
}