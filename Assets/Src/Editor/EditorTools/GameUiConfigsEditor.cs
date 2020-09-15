using GameLovers.UiService;
using GameLoversEditor.UiService;
using Ids;
using UnityEditor;

namespace EditorTools
{
	/// <summary>
	/// Games custom <see cref="UiConfigsEditor{TSet}"/>
	/// </summary>
	[CustomEditor(typeof(UiConfigs))]
	public class GameUiConfigsEditor : UiConfigsEditor<UiSetId>
	{
	}
}