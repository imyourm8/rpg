using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Attributes;

public class MobEditor : ConfigurationEditor.IEditorTab
{
	private Vector2 scrollPosition_;
	private EnemyEntry currentModel_;
	private List<EnemyEntry> models_;
	private int selectedSpell_;

	private static MobEditor instance_;
	public static MobEditor Instance()
	{
		return instance_;
	}
	
	public MobEditor()
	{
		instance_ = this;
		LootQuest.GameData.Enemies.Instance.Load ();
		models_ = LootQuest.GameData.Enemies.Instance.Data;
	}

	string ConfigurationEditor.IEditorTab.Title
	{ get { return "Enemies"; } }

	void ConfigurationEditor.IEditorTab.OnGUI () 
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Enemies.Instance.Save();
		}

		if (currentModel_ != null && GUILayout.Button ("Remove Enemy")) 
		{
			models_.Remove(currentModel_);
			currentModel_ = null;
		}

		if (GUILayout.Button ("Add Enemy")) 
		{
			currentModel_ = new EnemyEntry();
			models_.Add(currentModel_);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		DrawMonsterList ();

		if (currentModel_ != null)
			DrawNewMonsterSection ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	public void RemoveSpells(HashSet<string> spells)
	{
		foreach (var mob in models_) 
		{
			if (!spells.Contains(mob.autoAttackAbility))
			{
				mob.autoAttackAbility = "";
			}
		}
	}

	void DrawMonsterList()
	{
		EditorGUILayout.BeginVertical ();

		scrollPosition_ = EditorGUILayout.BeginScrollView (scrollPosition_, GUILayout.Width (300));

		int total = models_.Count;
		EnemyEntry nextModel = null;
		for (int i = 0; i < total; ++i) 
		{
			var model = models_[i];
			if (GUILayout.Button (model.ID, EditorHelper.ListSkin(model==currentModel_))) 
			{
				nextModel = model;
			}
		}

		if (nextModel != null) 
		{
			currentModel_ = nextModel;
		}

		EditorGUILayout.EndScrollView ();

		EditorGUILayout.EndVertical ();
	}

	void DrawNewMonsterSection()
	{
		EditorGUILayout.BeginVertical (GUILayout.Width (300));

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("ID", EditorStyles.label);
		EditorGUILayout.Separator ();
		currentModel_.ID = EditorGUILayout.TextField (currentModel_.ID);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal (); 
		var spellNames = SpellTab.Instance ().SpellNames;
		selectedSpell_ = EditorGUILayout.Popup ("Auto Attack Ability", selectedSpell_, spellNames);
		if (spellNames.Length > selectedSpell_)
			currentModel_.autoAttackAbility = spellNames[selectedSpell_];
		EditorGUILayout.EndHorizontal ();

		foreach (var att in currentModel_.Stats) 
		{
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label (att.id.ToString(), EditorStyles.label);
			EditorGUILayout.Separator ();
			EditorHelper.DrawRange(att);
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("View", EditorStyles.label);
		EditorGUILayout.Separator ();
		currentModel_.viewPrefab = (GameObject)EditorGUILayout.ObjectField (currentModel_.viewPrefab, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		currentModel_.ai = (LootQuest.Game.Units.AI.Type)EditorGUILayout.EnumPopup ("AI", currentModel_.ai);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndVertical ();
	}
}
