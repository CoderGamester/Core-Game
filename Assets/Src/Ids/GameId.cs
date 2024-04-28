using System.Collections.Generic;
using System.Collections.ObjectModel;

/* AUTO GENERATED CODE */
namespace Game.Ids
{
	public enum GameId
	{
		Random,
		SoftCurrency,
		HardCurrency,
		Weapon,
		Shield,
		Boots,
		Trash,
		HealthPotion,
		ManaPotion,
		TownHall,
		Farm,
		House
	}

	public enum GameIdGroup
	{
		GameDesign,
		Currency,
		Item,
		Equipment,
		Potion,
		Building
	}

	public static class GameIdLookup
	{
		public static bool IsInGroup(this GameId id, GameIdGroup group)
		{
			if (!_groups.TryGetValue(id, out var groups))
			{
				return false;
			}
			return groups.Contains(group);
		}

		public static IList<GameId> GetIds(this GameIdGroup group)
		{
			return _ids[group];
		}

		public static IList<GameIdGroup> GetGroups(this GameId id)
		{
			return _groups[id];
		}

		public class GameIdComparer : IEqualityComparer<GameId>
		{
			public bool Equals(GameId x, GameId y)
			{
				return x == y;
			}

			public int GetHashCode(GameId obj)
			{
				return (int)obj;
			}
		}

		public class GameIdGroupComparer : IEqualityComparer<GameIdGroup>
		{
			public bool Equals(GameIdGroup x, GameIdGroup y)
			{
				return x == y;
			}

			public int GetHashCode(GameIdGroup obj)
			{
				return (int)obj;
			}
		}

		private static readonly Dictionary<GameId, ReadOnlyCollection<GameIdGroup>> _groups =
			new Dictionary<GameId, ReadOnlyCollection<GameIdGroup>> (new GameIdComparer())
			{
				{
					GameId.Random, new List<GameIdGroup>
					{
						GameIdGroup.GameDesign
					}.AsReadOnly()
				},
				{
					GameId.SoftCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Currency
					}.AsReadOnly()
				},
				{
					GameId.HardCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Currency
					}.AsReadOnly()
				},
				{
					GameId.Weapon, new List<GameIdGroup>
					{
						GameIdGroup.Item,
						GameIdGroup.Equipment
					}.AsReadOnly()
				},
				{
					GameId.Shield, new List<GameIdGroup>
					{
						GameIdGroup.Item,
						GameIdGroup.Equipment
					}.AsReadOnly()
				},
				{
					GameId.Boots, new List<GameIdGroup>
					{
						GameIdGroup.Item,
						GameIdGroup.Equipment
					}.AsReadOnly()
				},
				{
					GameId.Trash, new List<GameIdGroup>
					{
						GameIdGroup.Item
					}.AsReadOnly()
				},
				{
					GameId.HealthPotion, new List<GameIdGroup>
					{
						GameIdGroup.Item,
						GameIdGroup.Potion
					}.AsReadOnly()
				},
				{
					GameId.ManaPotion, new List<GameIdGroup>
					{
						GameIdGroup.Item,
						GameIdGroup.Potion
					}.AsReadOnly()
				},
				{
					GameId.TownHall, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.Farm, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.House, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
			};

		private static readonly Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> _ids =
			new Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> (new GameIdGroupComparer())
			{
				{
					GameIdGroup.GameDesign, new List<GameId>
					{
						GameId.Random
					}.AsReadOnly()
				},
				{
					GameIdGroup.Currency, new List<GameId>
					{
						GameId.SoftCurrency,
						GameId.HardCurrency
					}.AsReadOnly()
				},
				{
					GameIdGroup.Item, new List<GameId>
					{
						GameId.Weapon,
						GameId.Shield,
						GameId.Boots,
						GameId.Trash,
						GameId.HealthPotion,
						GameId.ManaPotion
					}.AsReadOnly()
				},
				{
					GameIdGroup.Equipment, new List<GameId>
					{
						GameId.Weapon,
						GameId.Shield,
						GameId.Boots
					}.AsReadOnly()
				},
				{
					GameIdGroup.Potion, new List<GameId>
					{
						GameId.HealthPotion,
						GameId.ManaPotion
					}.AsReadOnly()
				},
				{
					GameIdGroup.Building, new List<GameId>
					{
						GameId.TownHall,
						GameId.Farm,
						GameId.House
					}.AsReadOnly()
				},
			};
	}
}
