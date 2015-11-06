using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ConfigurationEditor : EditorWindow 
{
	public interface IEditorTab
	{	
		void OnGUI ();
		string Title { get; }
	}

	[MenuItem ("Editors/Configuration")]
	public static void ShowWindow () 
	{
		EditorWindow.GetWindow(typeof(ConfigurationEditor));
	}

	private int tabIndex_ = 0;
	private List<IEditorTab> tabs_;
	private string[] tabsTitles_;

	public ConfigurationEditor()
	{
		tabs_ = new List<IEditorTab> ();

		EditorHelper.Init ();

		PopulateTabs ();

		tabsTitles_ = new string[tabs_.Count];

		for(int i = 0; i < tabs_.Count; ++i) 
		{
			tabsTitles_[i] = tabs_[i].Title;
		}
	}

	void PopulateTabs()
	{ 
		tabs_.Clear ();
		tabs_.Add (new HeroEditor ());
		tabs_.Add (new MobEditor ());
		tabs_.Add (new EnemyTable ());
		tabs_.Add (new TowerSettings());
		tabs_.Add (new ItemEditor());
		tabs_.Add (new LootTables());
		tabs_.Add (new SpellEditor ());
	}

    void OnGUI()
    {
		if (GUILayout.Button ("Reload All")) 
		{
			PopulateTabs();
		}

		tabIndex_ = GUILayout.Toolbar (tabIndex_, tabsTitles_);

		if (tabIndex_ < tabs_.Count) 
		{
			var selectedTab = tabs_ [tabIndex_];
			selectedTab.OnGUI ();
		}
    }
}
