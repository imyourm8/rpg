using UnityEngine;
using UnityEditor;
using System.Collections;

public class HeroEditor : ConfigurationEditor.IEditorTab 
{
	private LootQuest.GameData.HeroEntry hero_;

	string ConfigurationEditor.IEditorTab.Title 
	{
		get { return "Hero"; }
	}

	public HeroEditor()
	{
		LootQuest.GameData.Heroes.Instance.Load ();
		hero_ = LootQuest.GameData.Heroes.Instance.Data;
	}

	void ConfigurationEditor.IEditorTab.OnGUI ()
	{
		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Heroes.Instance.Save();
		}

		DrawHero ();

		EditorGUILayout.EndHorizontal ();
	}

	void DrawHero()
	{
		EditorGUILayout.BeginVertical ();

		hero_.viewPrefab = (GameObject)EditorGUILayout.ObjectField (hero_.viewPrefab, typeof(GameObject));

		foreach (var att in hero_.Stats) 
		{
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label (att.ID.ToString(), EditorStyles.label);
			EditorGUILayout.Separator ();
			EditorHelper.DrawTextfieldValue(att);
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.EndVertical ();
	}
}
