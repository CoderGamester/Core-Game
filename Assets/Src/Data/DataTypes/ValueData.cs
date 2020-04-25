using System;
using Ids;

namespace Data.DataTypes
{
	[Serializable]
	public struct IntData
	{
		public GameId Key;
		public int Value;

		public IntData(GameId key, int value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{Key},{Value.ToString()}]";
		}
	}
	
	[Serializable]
	public struct FloatData
	{
		public GameId Key;
		public float Value;

		public FloatData(GameId key, float value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{Key},{Value.ToString()}]";
		}
	}
	
	[Serializable]
	public struct StringData
	{
		public GameId Key;
		public string Value;

		public StringData(GameId key, string value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{Key},{Value}]";
		}
	}
}