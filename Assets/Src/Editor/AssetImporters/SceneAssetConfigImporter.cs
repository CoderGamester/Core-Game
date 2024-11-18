using Game.Configs;
using Game.Ids;
using GameLoversEditor.AssetsImporter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Editor.AssetImporters
{
	/// <summary>
	/// This importer is responsible to setup the weak references to the scenes that match the <see cref="SceneId"/> names
	/// </summary>
	public class SceneAssetConfigImporter : AssetsConfigsImporter<SceneId, Scene, SceneAssetConfigs>
	{
	}
}
