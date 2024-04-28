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
		Addressables_Configs_UiConfigs,
		Addressables_Ui_Loading_Screen,
		Addressables_Ui_MainHud
	}

	public enum AddressableLabel
	{
		Label_GenerateIds
	}

	public static class AddressablePathLookup
	{
		public static readonly string AddressablesUi = "Addressables/Ui";
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
					new AddressableConfig(0, "Addressables/Ui/MainHud.prefab", "Assets/Addressables/Ui/MainHud.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(1, "Addressables/Ui/Loading Screen.prefab", "Assets/Addressables/Ui/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
					new AddressableConfig(2, "Addressables/Configs/UiConfigs.asset", "Assets/Addressables/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {"GenerateIds"}),
					new AddressableConfig(3, "Addressables/Configs/GameConfigs.asset", "Assets/Addressables/Configs/GameConfigs.asset", typeof(Game.Configs.GameConfigs), new [] {"GenerateIds"}),
					new AddressableConfig(4, "Addressables/Configs/DataConfigs.asset", "Assets/Addressables/Configs/DataConfigs.asset", typeof(Game.Configs.DataConfigs), new [] {"GenerateIds"}),
				}.AsReadOnly()}
		});

		private static readonly IList<AddressableConfig> _addressableConfigs = new List<AddressableConfig>
		{
			new AddressableConfig(0, "Addressables/Configs/DataConfigs.asset", "Assets/Addressables/Configs/DataConfigs.asset", typeof(Game.Configs.DataConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(1, "Addressables/Configs/GameConfigs.asset", "Assets/Addressables/Configs/GameConfigs.asset", typeof(Game.Configs.GameConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(2, "Addressables/Configs/UiConfigs.asset", "Assets/Addressables/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {"GenerateIds"}),
			new AddressableConfig(3, "Addressables/Ui/Loading Screen.prefab", "Assets/Addressables/Ui/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"}),
			new AddressableConfig(4, "Addressables/Ui/MainHud.prefab", "Assets/Addressables/Ui/MainHud.prefab", typeof(UnityEngine.GameObject), new [] {"GenerateIds"})
		}.AsReadOnly();
	}
}
