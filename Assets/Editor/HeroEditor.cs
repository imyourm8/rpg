using UnityEngine;
using UnityEditor;
using System.Collections;

public class HeroEditor : ConfigurationEditor.IEditorTab 
{
	private LootQuest.GameData.HeroEntry hero_;
	private int selectedSpell_ = 0;

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

		EditorGUILayout.BeginHorizontal (); 
		var spellNames = SpellTab.Instance ().SpellNames;
		selectedSpell_ = EditorGUILayout.Popup ("Auto Attack Ability", selectedSpell_, spellNames);
		if (spellNames.Length > selectedSpell_)
			hero_.autoAttackAbility = spellNames[selectedSpell_];
		EditorGUILayout.EndHorizontal ();

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
