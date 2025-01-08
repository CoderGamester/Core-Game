/* AUTO GENERATED CODE */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameLovers.AssetsImporter;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Game.Ids
{
	public enum AddressableId
	{
		Addressables_Configs_DataConfigs,
		Addressables_Configs_GameConfigs,
		Addressables_Configs_SceneAssetConfigs,
		Addressables_Configs_UiConfigs,
		Addressables_Prefabs_UI_Compliance_Screen,
		Addressables_Prefabs_UI_GameOver,
		Addressables_Prefabs_UI_Loading_Screen,
		Addressables_Prefabs_UI_Main_Menu,
		Addressables_Prefabs_UI_MainHud,
		Addressables_Prefabs_UI_Pause_PopUp,
		Addressables_Scenes_Game,
		Addressables_Scenes_Menu
	}

	public enum AddressableLabel
	{
		Label_GenerateIds
	}

	public static class AddressablePathLookup
	{
		public static readonly string AddressablesScenes = "Addressables/Scenes";
		public static readonly string AddressablesPrefabsUI = "Addressables/Prefabs/UI";
		public static readonly string AddressablesConfigs = "Addressables/Configs";
	}

	public static class AddressableConfigLookup
	{
		public static IList<AddressableConfig> Configs => _addressableConfigs;
		public static IList<string> Labels => _addressableLabels;

		public static AddressableConfig GetConfig(this AddressableId addressable)
		{
			return _addressableConfigs[(int) addressable];
		}

		public static IList<AddressableConfig> GetConfigs(this AddressableLabel label)
		{
			return _addressableLabelMap[_addressableLabels[(int) label]];
		}

		public static IList<AddressableConfig> GetConfigs(string label)
		{
			return _addressableLabelMap[label];
		}

		public static string ToLabelString(this AddressableLabel label)
		{
			return _addressableLabels[(int) label];
		}

		private static readonly IList<string> _addressableLabels = new List<string>
		{
			"GenerateIds"
		}.AsReadOnly();

		private static readonly IReadOnlyDictionary<string, IList<AddressableConfig>> _addressableLabelMap = new ReadOnlyDictionary<string, IList<AddressableConfig>>(new Dictionary<string, IList<AddressableConfig>>
		{
			{"GenerateIds", new List<AddressableConfig>
				{
					new AddressableConfig(0, "Addressables/Scenes/Menu.unity", "Assets/Addressables/Scenes/Menu.unity", typeof(UnityEngine.SceneManagement.Scene), new [] {"GenerateIds"}),
					new AddressableConfig(1, "Addressables/Scenes/Game.unity", "Assets/Addressables/Scenes/Game.unity", typeof(UnityEngine.SceneManagement.Scene), new [] {"GenerateIds"}),
					new AddressableConfig(2, "Addressables/Prefabs/UI/Pause PopUp.prefab", "Assets/Addressables/Prefabs/UI/Pause PopUp.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(3, "Addressables/Prefabs/UI/MainHud.prefab", "Assets/Addressables/Prefabs/UI/MainHud.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(4, "Addressables/Prefabs/UI/Main Menu.prefab", "Assets/Addressables/Prefabs/UI/Main Menu.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(5, "Addressables/Prefabs/UI/Loading Screen.prefab", "Assets/Addressables/Prefabs/UI/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(6, "Addressables/Prefabs/UI/GameOver.prefab", "Assets/Addressables/Prefabs/UI/GameOver.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(7, "Addressables/Prefabs/UI/Compliance Screen.prefab", "Assets/Addressables/Prefabs/UI/Compliance Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(8, "Addressables/Configs/UiConfigs.asset", "Assets/Addressables/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {"GenerateIds"}),
					new AddressableConfig(9, "Addressables/Configs/SceneAssetConfigs.asset", "Assets/Addressables/Configs/SceneAssetConfigs.asset", typeof(Game.Configs.SceneAssetConfigs), new [] {"GenerateIds"}),
					new AddressableConfig(10, "Addressables/Configs/GameConfigs.asset", "Assets/Addressables/Configs/GameConfigs.asset", typeof(Game.Configs.GameConfigs), new [] {"GenerateIds"}),
					new AddressableConfig(11, "Addressables/Configs/DataConfigs.asset", "Assets/Addressables/Configs/DataConfigs.asset", typeof(Game.Configs.DataConfigs), new [] {"GenerateIds"}),
				}.AsReadOnly()}
		});

		private static readonly IList<AddressableConfig> _addressableConfigs = new List<AddressableConfig>
		{
			new AddressableConfig(0, "Addressables/Configs/DataConfigs.asset", "Assets/Addressables/Configs/DataConfigs.asset", typeof(Game.Configs.DataConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(1, "Addressables/Configs/GameConfigs.asset", "Assets/Addressables/Configs/GameConfigs.asset", typeof(Game.Configs.GameConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(2, "Addressables/Configs/SceneAssetConfigs.asset", "Assets/Addressables/Configs/SceneAssetConfigs.asset", typeof(Game.Configs.SceneAssetConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(3, "Addressables/Configs/UiConfigs.asset", "Assets/Addressables/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(4, "Addressables/Prefabs/UI/Compliance Screen.prefab", "Assets/Addressables/Prefabs/UI/Compliance Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(5, "Addressables/Prefabs/UI/GameOver.prefab", "Assets/Addressables/Prefabs/UI/GameOver.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(6, "Addressables/Prefabs/UI/Loading Screen.prefab", "Assets/Addressables/Prefabs/UI/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(7, "Addressables/Prefabs/UI/Main Menu.prefab", "Assets/Addressables/Prefabs/UI/Main Menu.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(8, "Addressables/Prefabs/UI/MainHud.prefab", "Assets/Addressables/Prefabs/UI/MainHud.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(9, "Addressables/Prefabs/UI/Pause PopUp.prefab", "Assets/Addressables/Prefabs/UI/Pause PopUp.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(10, "Addressables/Scenes/Game.unity", "Assets/Addressables/Scenes/Game.unity", typeof(UnityEngine.SceneManagement.Scene), new [] {"GenerateIds"}),
			new AddressableConfig(11, "Addressables/Scenes/Menu.unity", "Assets/Addressables/Scenes/Menu.unity", typeof(UnityEngine.SceneManagement.Scene), new [] {"GenerateIds"})
		}.AsReadOnly();
	}
}
