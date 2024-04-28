using System.Collections.Generic;
using System.IO;
using System.Text;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;
using UnityEditor;

namespace Game.SheetImporters
{
	/// <inheritdoc />
	[GoogleSheetImportOrder(0)]
	public class GameIdsImporter : IGoogleSheetConfigsImporter
	{
		private const string _NAME = "GameId";
		private const string _NAME_GROUP = "GameIdGroup";
		private const string _ID_TAG = "Id";
		private const string _GROUPS_TAG = "Groups";
		
		/// <inheritdoc />
		public string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1095NlqM_3cvZfCTnQtTsCkOAXh0McypKZje47WtBzPY/edit#gid=0";
		
		/// <inheritdoc />
		public void Import(List<Dictionary<string, string>> data)
		{
			var idList = new List<string>();
			var groupList = new List<string>();
			var mapGroups = new Dictionary<string, List<string>>();
			var mapIds = new Dictionary<string, List<string>>();
			
			foreach (var entry in data)
			{
				var groups = CsvParser.ArrayParse<string>(entry[_GROUPS_TAG]);
				var id = GetCleanName(entry[_ID_TAG]);
				
				idList.Add(id);
				mapGroups.Add(id, groups);

				foreach (var group in groups)
				{
					var groupName = GetCleanName(group);
					if (!groupList.Contains(groupName))
					{
						groupList.Add(groupName);
						mapIds.Add(groupName, new List<string>());
					}
					
					mapIds[groupName].Add(id);
				}
			}

			var script = GenerateScript(idList, groupList, mapGroups, mapIds);

			SaveScript(script);
			AssetDatabase.Refresh();
		}

		private static string GenerateScript(IList<string> ids, IList<string> groups, Dictionary<string, List<string>> mapGroups,
			Dictionary<string, List<string>> mapIds)
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("using System.Collections.Generic;");
			stringBuilder.AppendLine("using System.Collections.ObjectModel;");
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine("/* AUTO GENERATED CODE */");
			stringBuilder.AppendLine("namespace Game.Ids");
			stringBuilder.AppendLine("{");
			
			stringBuilder.AppendLine($"\tpublic enum {_NAME}");
			stringBuilder.AppendLine("\t{");
			GenerateEnums(stringBuilder, ids);
			stringBuilder.AppendLine("\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\tpublic enum {_NAME_GROUP}");
			stringBuilder.AppendLine("\t{");
			GenerateEnums(stringBuilder, groups);
			stringBuilder.AppendLine("\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\tpublic static class {_NAME}Lookup");
			stringBuilder.AppendLine("\t{");
			GenerateLoopUpMethods(stringBuilder);
			GenerateLoopUpMaps(stringBuilder, mapGroups, _NAME, _NAME_GROUP, "groups");
			GenerateLoopUpMaps(stringBuilder, mapIds, _NAME_GROUP, _NAME, "ids");
			stringBuilder.AppendLine("\t}");
			
			stringBuilder.AppendLine("}");

			return stringBuilder.ToString();
		}

		private static void GenerateLoopUpMethods(StringBuilder stringBuilder)
		{
			stringBuilder.AppendLine($"\t\tpublic static bool IsInGroup(this {_NAME} id, {_NAME_GROUP} group)");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine("\t\t\tif (!_groups.TryGetValue(id, out var groups))");
			stringBuilder.AppendLine("\t\t\t{");
			stringBuilder.AppendLine("\t\t\t\treturn false;");
			stringBuilder.AppendLine("\t\t\t}");
			stringBuilder.AppendLine("\t\t\treturn groups.Contains(group);");
			stringBuilder.AppendLine("\t\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\tpublic static IList<{_NAME}> GetIds(this {_NAME_GROUP} group)");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine("\t\t\treturn _ids[group];");
			stringBuilder.AppendLine("\t\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\tpublic static IList<{_NAME_GROUP}> GetGroups(this {_NAME} id)");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine("\t\t\treturn _groups[id];");
			stringBuilder.AppendLine("\t\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\tpublic class {_NAME}Comparer : IEqualityComparer<{_NAME}>");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine($"\t\t\tpublic bool Equals({_NAME} x, {_NAME} y)");
			stringBuilder.AppendLine("\t\t\t{");
			stringBuilder.AppendLine("\t\t\t\treturn x == y;");
			stringBuilder.AppendLine("\t\t\t}");
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\t\tpublic int GetHashCode({_NAME} obj)");
			stringBuilder.AppendLine("\t\t\t{");
			stringBuilder.AppendLine("\t\t\t\treturn (int)obj;");
			stringBuilder.AppendLine("\t\t\t}");
			stringBuilder.AppendLine("\t\t}");
			
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\tpublic class {_NAME_GROUP}Comparer : IEqualityComparer<{_NAME_GROUP}>");
			stringBuilder.AppendLine("\t\t{");
			stringBuilder.AppendLine($"\t\t\tpublic bool Equals({_NAME_GROUP} x, {_NAME_GROUP} y)");
			stringBuilder.AppendLine("\t\t\t{");
			stringBuilder.AppendLine("\t\t\t\treturn x == y;");
			stringBuilder.AppendLine("\t\t\t}");
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\t\tpublic int GetHashCode({_NAME_GROUP} obj)");
			stringBuilder.AppendLine("\t\t\t{");
			stringBuilder.AppendLine("\t\t\t\treturn (int)obj;");
			stringBuilder.AppendLine("\t\t\t}");
			stringBuilder.AppendLine("\t\t}");
		}

		private static void GenerateLoopUpMaps(StringBuilder stringBuilder, Dictionary<string, List<string>> map,
			string element1Type, string element2Type, string fieldName)
		{
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine($"\t\tprivate static readonly Dictionary<{element1Type}, ReadOnlyCollection<{element2Type}>> _{fieldName} =");
			stringBuilder.AppendLine($"\t\t\tnew Dictionary<{element1Type}, ReadOnlyCollection<{element2Type}>> (new {element1Type}Comparer())");
			stringBuilder.AppendLine("\t\t\t{");

			foreach (var pair in map)
			{
				stringBuilder.AppendLine("\t\t\t\t{");
				stringBuilder.AppendLine($"\t\t\t\t\t{element1Type}.{pair.Key}, new List<{element2Type}>");
				stringBuilder.AppendLine("\t\t\t\t\t{");
				for (var i = 0; i < pair.Value.Count; i++)
				{
					stringBuilder.Append("\t\t\t\t\t\t");
					stringBuilder.Append($"{element2Type}.{pair.Value[i]}");
					stringBuilder.Append(i + 1 == pair.Value.Count ? "\n" : ",\n");
				}
				stringBuilder.AppendLine("\t\t\t\t\t}.AsReadOnly()");
				stringBuilder.AppendLine("\t\t\t\t},");
			}
			
			stringBuilder.AppendLine("\t\t\t};");
		}

		private static void GenerateEnums(StringBuilder stringBuilder, IList<string> list)
		{
			for (var i = 0; i < list.Count; i++)
			{
				stringBuilder.Append("\t\t");
				stringBuilder.Append(list[i]);
				stringBuilder.Append(i + 1 == list.Count ? "\n" : ",\n");
			}
		}
		
		private static string GetCleanName(string name)
		{
			return name.Replace(' ', '_');
		}

		private static void SaveScript(string scriptString)
		{
			var scriptAssets = AssetDatabase.FindAssets($"t:Script {_NAME}");
			var scriptPath = $"Assets/{_NAME}.cs";

			foreach (var scriptAsset in scriptAssets)
			{
				var path = AssetDatabase.GUIDToAssetPath(scriptAsset);
				if (path.EndsWith($"/{_NAME}.cs"))
				{
					scriptPath = path;
					break;
				}
			}

			File.WriteAllText(scriptPath, scriptString);
		}
	}
}