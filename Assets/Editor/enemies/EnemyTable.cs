using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using LootQuest.GameData;

public class EnemyTable : ConfigurationEditor.IEditorTab 
{
	public string Title {
		get {return "Enemy tables";} 
	}

	private static EnemyTable instance_;

	private List<EnemyTableEntry> tables_;
	private Vector2 tablesScrollPos_ = Vector2.zero;
	private EnemyTableEntry currentTable_;

	public string[] TableNames = new string[]{};
	public Dictionary<string, EnemyTableEntry> TableHash = new Dictionary<string, EnemyTableEntry>();

	public static EnemyTable Instance() 
	{
		return instance_;
	}

	public EnemyTable()
	{
		LootQuest.GameData.EnemyTables.Instance.Load ();
		tables_ = LootQuest.GameData.EnemyTables.Instance.Data;
		instance_ = this;
		RefreshTables ();
	}

	private void RefreshTables()
	{
		TableHash.Clear ();
		TableNames = new string[tables_.Count];

		int count = tables_.Count;
		for (int i = 0; i < count; ++i)
		{
			TableNames[i] = tables_[i].id;
			TableHash.Add(TableNames[i], tables_[i]);
		}
	}

	private void RefreshEnemies()
	{
		var names = MobEditor.Instance().EnemyNames;
	}

	public void OnGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.EnemyTables.Instance.Save();
		}
		if (GUILayout.Button ("Refresh Enemies")) 
		{
			RefreshEnemies();
		}
		if (GUILayout.Button ("Add table")) 
		{
			currentTable_ = new EnemyTableEntry();
			tables_.Add(currentTable_);
			RefreshTables();
		}
		if (currentTable_ != null && GUILayout.Button ("Remove table")) 
		{
			tables_.Remove(currentTable_);
			currentTable_ = null;
			RefreshTables();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();

		DrawTables ();

		if (currentTable_ != null) 
		{
			DrawCurrentTable();
		}

		EditorGUILayout.EndHorizontal ();
	}

	private void DrawTables()
	{
		EditorGUILayout.BeginScrollView(tablesScrollPos_, GUILayout.MaxWidth(500));

		EnemyTableEntry nextTable = null;
		foreach (var table in tables_) 
		{
			string id = table.id+" Lv: "+table.maxTowerLevel.ToString();
			if (GUILayout.Button(id, EditorHelper.ListSkin(table==currentTable_)))
			{
				nextTable = table;
			}
		}

		if (nextTable != null) 
		{
			currentTable_ = nextTable;
		}

		EditorGUILayout.EndScrollView ();
	}

	private void DrawCurrentTable()
	{
		EditorGUILayout.BeginVertical (GUILayout.MaxWidth (500));

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("ID");
		EditorGUILayout.Separator ();
		currentTable_.id = EditorGUILayout.TextField (currentTable_.id);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Max Tower Level");
		EditorGUILayout.Separator ();
		currentTable_.maxTowerLevel = EditorGUILayout.IntField (currentTable_.maxTowerLevel);
		EditorGUILayout.EndHorizontal ();

		EditorHelper.Draw ("Group Size", currentTable_.groupSize);
		EditorHelper.Draw ("Group Count", currentTable_.groupCount);

		GUILayout.Box("", EditorHelper.Line);

		foreach (var spawnChance in currentTable_.spawnChances) 
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Weight");
			EditorGUILayout.Separator ();
			spawnChance.weight = EditorGUILayout.IntField (spawnChance.weight);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Type");
			EditorGUILayout.Separator ();
			EditorGUILayout.LabelField (spawnChance.enemyType.ToString());
			EditorGUILayout.EndHorizontal ();

			EditorHelper.FoldoutContent.text = "Enemies";
			spawnChance.expanded = EditorGUILayout.Foldout (spawnChance.expanded, EditorHelper.FoldoutContent);

			if (spawnChance.expanded)
			{
				EditorGUILayout.BeginHorizontal ();
				var enemyNames = MobEditor.Instance().EnemyNames[spawnChance.enemyType];
				var enemyList = MobEditor.Instance().Enemies[spawnChance.enemyType];
				var enemies = currentTable_.enemies[spawnChance.enemyType];

				if (enemyNames.Length > spawnChance.nameIndex)
				{
					spawnChance.nameIndex = EditorGUILayout.Popup(spawnChance.nameIndex, enemyNames);
					if (GUILayout.Button(EditorHelper.PlusIcon, EditorHelper.PlusButton))
					{
						var name = enemyNames[spawnChance.nameIndex];
						var e = new EnemyTableSpawnEntry();
						e.enemy = enemyList[name];
						enemies.enemies.Add(e);
					}
				}

				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel++;

				EnemyTableSpawnEntry enemyToDel = null;
				int count = enemies.enemies.Count;
				for (int i = 0; i < count; ++i)
				{
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField(enemies.enemies[i].enemy.ID);
					if (GUILayout.Button("delete"))
					{
						enemyToDel = enemies.enemies[i];
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField("weight");
					EditorGUILayout.Separator();
					enemies.enemies[i].weight = EditorGUILayout.FloatField(enemies.enemies[i].weight);
					EditorGUILayout.EndHorizontal();

					if (i < count-1)
						GUILayout.Box("", EditorHelper.Line);
				}
				if (enemyToDel != null)
				{
					enemies.enemies.Remove(enemyToDel);
				}
				EditorGUI.indentLevel--;
			}

			GUILayout.Box("", EditorHelper.Line);
		}

		EditorGUILayout.EndVertical ();
	}
}
