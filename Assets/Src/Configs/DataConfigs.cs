using System;
using System.Collections.Generic;
using GameLovers;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct DataConfig
	{
		public GameId Id;
		public int Int;
		public string String;
		public float Float;
		public List<int> List;
		public IntPairData IntPair;
		public FloatPairData FloatPair;
		public StringPairData StringPair;
		public List<IntPairData> ListPair;
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
		public List<DataConfig> Configs => _configs;
	}
}