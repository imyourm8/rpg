using UnityEngine;
using System.Collections;

public class ItemEditor : ConfigurationEditor.IEditorTab
{
	string ConfigurationEditor.IEditorTab.Title {
		get {
			return "Items";
		}
	}

	void ConfigurationEditor.IEditorTab.OnGUI ()
	{

	}
}
