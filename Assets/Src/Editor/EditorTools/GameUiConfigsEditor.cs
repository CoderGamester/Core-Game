using GameLovers.UiService;
using GameLoversEditor.UiService;
using Game.Ids;
using UnityEditor;

namespace Game.EditorTools
{
	/// <summary>
	/// Games custom <see cref="UiConfigsEditor{TSet}"/>
	/// </summary>
	[CustomEditor(typeof(UiConfigs))]
	public class GameUiConfigsEditor : UiConfigsEditor<UiSetId>
	{
	}
}