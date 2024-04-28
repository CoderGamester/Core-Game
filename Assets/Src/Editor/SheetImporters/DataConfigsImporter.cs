using Game.Configs;
using GameLoversEditor.GoogleSheetImporter;

namespace Game.SheetImporters
{
	/// <inheritdoc />
	public class DataConfigsImporter : GoogleSheetConfigsImporter<DataConfig, DataConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1095NlqM_3cvZfCTnQtTsCkOAXh0McypKZje47WtBzPY/edit#gid=558488968";
	}
}