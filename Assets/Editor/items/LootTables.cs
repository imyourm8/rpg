using UnityEngine;
using UnityEditor;
using System.Collections;

public class LootTables : ConfigurationEditor.IEditorTab
{
	#region IEditorTab implementation

	string ConfigurationEditor.IEditorTab.Title {
		get {
			return "Loot";
		}
	}

	#endregion

	void ConfigurationEditor.IEditorTab.OnGUI () 
	{

	}
}
