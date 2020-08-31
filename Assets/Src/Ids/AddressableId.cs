/* AUTO GENERATED CODE */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameLovers.AddressablesExtensions;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Ids
{
	public enum AddressableId
	{
		Configs_DataConfigs,
		Configs_GameConfigs,
		Configs_UiConfigs,
		Prefabs_Ui_Hud,
		Prefabs_Ui_Loading_Screen
	}

	public enum AddressableLabel
	{
	}

	public static class AddressablePathLookup
	{
		public static readonly string Configs = "Configs";
		public static readonly string PrefabsUi = "Prefabs/Ui";
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

		private static readonly IList<string> _addressableLabels = new List<string>
		{
		}.AsReadOnly();

		private static readonly IReadOnlyDictionary<string, IList<AddressableConfig>> _addressableLabelMap = new ReadOnlyDictionary<string, IList<AddressableConfig>>(new Dictionary<string, IList<AddressableConfig>>
		{
		});

		private static readonly IList<AddressableConfig> _addressableConfigs = new List<AddressableConfig>
		{
			new AddressableConfig(0, "Configs/DataConfigs.asset", "Assets/ScriptableObjects/Configs/DataConfigs.asset", typeof(Configs.DataConfigs), new [] {""}),
			new AddressableConfig(1, "Configs/GameConfigs.asset", "Assets/ScriptableObjects/Configs/GameConfigs.asset", typeof(Configs.GameConfigs), new [] {""}),
			new AddressableConfig(2, "Configs/UiConfigs.asset", "Assets/ScriptableObjects/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {""}),
			new AddressableConfig(3, "Prefabs/Ui/Hud.prefab", "Assets/Art/Prefabs/Ui/Hud.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(4, "Prefabs/Ui/Loading Screen.prefab", "Assets/Art/Prefabs/Ui/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {""})
		}.AsReadOnly();
	}
}
