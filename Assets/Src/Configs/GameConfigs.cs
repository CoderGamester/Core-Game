using System;
using GameLovers;
using GameLovers.ConfigsProvider;
using UnityEngine;

namespace Game.Configs
{
	[Serializable]
	public struct GameConfig
	{
		public Pair<int, int> Random;
		public string GameVersion;
		public int MaxPlayers;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="GameConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "GameConfigs", menuName = "ScriptableObjects/Configs/GameConfigs")]
	public class GameConfigs : ScriptableObject, ISingleConfigContainer<GameConfig>
	{
		[SerializeField]
		private GameConfig _config;

		public GameConfig Config
		{
			get => _config;
			set => _config = value;
		}
	}
}