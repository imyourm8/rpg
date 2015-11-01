using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using LootQuest.GameData;

public class TowerSettings : ConfigurationEditor.IEditorTab
{
	private List<TowerEntry> towers_;
	private TowerEntry currentModel_;
	private StringBuilder strBuild_ = new StringBuilder();

	public TowerSettings()
	{
		Towers.Instance.Load ();
		towers_ = Towers.Instance.Data;
	}

	string ConfigurationEditor.IEditorTab.Title 
	{
		get { return "Towers"; }
	}

	void ConfigurationEditor.IEditorTab.OnGUI ()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) 
		{
			Towers.Instance.Save();
		}

		if (GUILayout.Button ("Add tower")) 
		{
			currentModel_ = new TowerEntry();
			towers_.Add(currentModel_);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();

		EditorGUILayout.BeginHorizontal ();
		DrawTowerList ();

		if (currentModel_ != null) 
		{
			DrawTower(currentModel_);
		}

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	void DrawTowerList ()
	{
		EditorGUILayout.BeginVertical ();

		TowerEntry nextModel = null;
		int count = towers_.Count;

		for (int i = 0; i < count; ++i) 
		{
			var model = towers_[i];
			strBuild_.Remove(0, strBuild_.Length);
			string label = strBuild_.Append("Level ").Append(model.LevelRange.Min).Append(" - ").Append(model.LevelRange.Max).ToString();

			if (GUILayout.Button(label,EditorHelper.ListSkin(model==currentModel_)))
			{
				nextModel = model;
			}
		}

		if (nextModel != null) 
		{
			currentModel_ = nextModel;
		}

		EditorGUILayout.EndVertical ();
	}

	void DrawTower (TowerEntry tower_)
	{
		EditorGUILayout.BeginVertical ();

		var lvlRange = tower_.LevelRange;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Level ", EditorStyles.label);
		EditorGUILayout.Separator ();
		lvlRange.Min = EditorHelper.DrawTextfieldValue(tower_.LevelRange.Min);
		GUILayout.Label ("-", EditorStyles.label);
		lvlRange.Max = EditorHelper.DrawTextfieldValue(tower_.LevelRange.Max);
		EditorGUILayout.EndHorizontal ();
		tower_.LevelRange = lvlRange;

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("HP multipler", EditorStyles.label);
		EditorGUILayout.Separator ();
		tower_.EnemyHpMultiplier = EditorHelper.DrawTextfieldValue(tower_.EnemyHpMultiplier);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Damage multipler", EditorStyles.label);
		EditorGUILayout.Separator ();
		tower_.EnemyDamageMultiplier = EditorHelper.DrawTextfieldValue(tower_.EnemyDamageMultiplier);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}
}
