using System;
using System.Collections.Generic;
using GameLovers;
using GameLovers.ConfigsProvider;
using Game.Ids;
using UnityEngine;

namespace Game.Configs
{
	[Serializable]
	public struct DataConfig
	{
		public GameId Id;
		public int Int;
		public string String;
		public float Float;
		public List<int> List;
		public Pair<int, int> IntPair;
		public Pair<float, float> FloatPair;
		public Pair<string, string> StringPair;
		public List<Pair<int, int>> ListPair;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="DataConfigs"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "DataConfigs", menuName = "ScriptableObjects/Configs/DataConfigs")]
	public class DataConfigs : ScriptableObject, IConfigsContainer<DataConfig>
	{
		[SerializeField]
		private List<DataConfig> _configs = new List<DataConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<DataConfig> Configs { get => _configs; set => _configs = value; }
	}
}