using Game.Ids;
using GameLovers.AssetsImporter;
using UnityEngine.SceneManagement;

namespace Game.Configs
{
	/// <summary>
	/// This config <see cref="AssetConfigsScriptableObject{TKey, TValue}"/> contains all the weak references to the scenes in this project
	/// </summary>
	public class SceneAssetConfigs : AssetConfigsScriptableObject<SceneId, Scene>
	{
	}
}
