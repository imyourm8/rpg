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
	private Vector2 scrollPos_ = Vector2.zero;

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

		if (currentModel_ != null && GUILayout.Button ("Remove tower")) 
		{
			towers_.Remove(currentModel_);
			currentModel_ = null;
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
		EditorGUILayout.BeginVertical (GUILayout.MaxWidth (100));

		TowerEntry nextModel = null;
		int count = towers_.Count;

		for (int i = 0; i < count; ++i) 
		{
			var model = towers_[i];
			strBuild_.Remove(0, strBuild_.Length);
			string label = strBuild_.Append("Level ").Append(model.LevelRange.Min).Append(" - ").Append(model.LevelRange.Max).ToString();

			if (GUILayout.Button(label, EditorHelper.ListSkin(model==currentModel_)))
			{
				nextModel = model;
			}
		}

		if (nextModel != null) 
		{
			currentModel_ = nextModel;
			scrollPos_ = Vector2.zero;
		}

		EditorGUILayout.EndVertical ();
	}

	void DrawTower (TowerEntry tower_)
	{
		EditorGUILayout.BeginVertical (GUILayout.MaxWidth (500));

		var lvlRange = tower_.LevelRange;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Level ", EditorStyles.label);
		EditorGUILayout.Separator ();
		lvlRange.Min = EditorGUILayout.IntField(tower_.LevelRange.Min);
		lvlRange.Min = Math.Max (1, lvlRange.Min);
		GUILayout.Label ("-", EditorStyles.label);
		lvlRange.Max = EditorGUILayout.IntField(tower_.LevelRange.Max);
		lvlRange.Max = Math.Max (lvlRange.Min, lvlRange.Max);
		EditorGUILayout.EndHorizontal ();
		tower_.LevelRange = lvlRange;

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("HP multipler", EditorStyles.label);
		EditorGUILayout.Separator ();
		tower_.EnemyHpMultiplier = EditorGUILayout.FloatField(tower_.EnemyHpMultiplier);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Damage multipler", EditorStyles.label);
		EditorGUILayout.Separator ();
		tower_.EnemyDamageMultiplier = EditorGUILayout.FloatField(tower_.EnemyDamageMultiplier);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.LabelField("Enemy tables");
		EditorGUILayout.BeginHorizontal ();
		var tableNames = EnemyTable.Instance ().TableNames;
		if (tableNames.Length > tower_.selectedEnemyTable)
		{
			tower_.selectedEnemyTable = EditorGUILayout.Popup(tower_.selectedEnemyTable, tableNames);
			if (GUILayout.Button(EditorHelper.PlusIcon, EditorHelper.PlusButton))
			{
				var name = tableNames[tower_.selectedEnemyTable];
				tower_.EnemyTables.Add(EnemyTable.Instance ().TableHash[name]);
			}
		}
		EditorGUILayout.EndHorizontal ();

		scrollPos_ = EditorGUILayout.BeginScrollView (scrollPos_, GUILayout.MaxHeight (600));
		EditorGUI.indentLevel++;
		EnemyTableEntry tableToDelete = null;
		foreach (var table in tower_.EnemyTables) 
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField(table.id);
			EditorGUILayout.Separator();
			if (GUILayout.Button(EditorHelper.TrashCanIcon, EditorHelper.TrashCanButton))
			{
				tableToDelete = table;
			}
			EditorGUILayout.EndHorizontal ();
		}

		if (tableToDelete != null)
		{
			tower_.EnemyTables.Remove(tableToDelete);
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.EndVertical ();
	}
}
