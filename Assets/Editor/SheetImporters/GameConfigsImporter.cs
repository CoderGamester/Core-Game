using System.Collections.Generic;
using Configs;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;

namespace SheetImporters
{
	/// <inheritdoc />
	public class GameConfigsImporter : GoogleSheetSingleConfigImporter<GameConfig, GameConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1095NlqM_3cvZfCTnQtTsCkOAXh0McypKZje47WtBzPY/edit#gid=1963922464";

		/// <inheritdoc />
		protected override GameConfig Deserialize(List<Dictionary<string, string>> data)
		{
			var config = new GameConfig() as object;
			var type = typeof(GameConfig);

			foreach (var row in data)
			{
				var field = type.GetField(row["Key"]);
				
				field.SetValue(config, CsvParser.Parse(row["Value"], field.FieldType));
			}
			
			return (GameConfig) config;
		}
	}
}