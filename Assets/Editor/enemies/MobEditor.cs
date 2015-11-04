using UnityEngine;
using UnityEditor;

using System;
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

	public Dictionary<LootQuest.Game.Units.EnemyType, string[]> EnemyNames = new Dictionary<LootQuest.Game.Units.EnemyType, string[]>();
	public Dictionary<LootQuest.Game.Units.EnemyType, Dictionary<string, EnemyEntry>> Enemies = new Dictionary<LootQuest.Game.Units.EnemyType, Dictionary<string, EnemyEntry>> ();

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

		RefreshEnemyNames ();
	}

	private void RefreshEnemyNames()
	{
		EnemyNames.Clear ();
		Enemies.Clear ();
		Dictionary<LootQuest.Game.Units.EnemyType, List<EnemyEntry>> dict = new Dictionary<LootQuest.Game.Units.EnemyType, List<EnemyEntry>> ();

		foreach (LootQuest.Game.Units.EnemyType e in Enum.GetValues(typeof(LootQuest.Game.Units.EnemyType))) 
		{
			dict.Add(e, new List<EnemyEntry>());
		}

		int count = models_.Count;
		for (int i = 0; i < count; ++i) 
		{
			var model = models_[i];
			dict[model.type].Add(model);
		}

		foreach (var d in dict) 
		{
			var list = d.Value;
			var names = new string[list.Count];
			EnemyNames[d.Key] = names;

			var enemies = new Dictionary<string, EnemyEntry>();
			Enemies.Add(d.Key, enemies);

			int c = list.Count;
			for (int k = 0; k < c; ++k)
			{
				names[k] = list[k].ID;
				enemies.Add(list[k].ID, list[k]);
			}
		}
	}

	private void CheckEnemySpells()
	{
		var hash = SpellTab.Instance ().SpellHash;

		foreach (var enemy in models_) 
		{
			foreach(var entry in enemy.spells)
			{
				entry.spells.RemoveAll(x=>!hash.Contains(x.spellID));
			}
		}
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
			RefreshEnemyNames();
		}

		if (GUILayout.Button ("Check")) 
		{
			CheckEnemySpells();
		}

		if (GUILayout.Button ("Add Enemy")) 
		{
			currentModel_ = new EnemyEntry();
			models_.Add(currentModel_);
			RefreshEnemyNames();
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		DrawMonsterList ();

		if (currentModel_ != null) 
		{
			DrawSpells();
			DrawNewMonsterSection ();
		}

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

	void DrawSpells()
	{
		EnemySpell spellToDelete= null;
		EnemySpellsEntry entryToDelete = null;
		EditorGUILayout.BeginVertical (GUILayout.MaxWidth(600));
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add spells")) 
		{
			var entry = new EnemySpellsEntry();
			currentModel_.spells.Add(entry);
		}
		EditorGUILayout.EndHorizontal ();
		foreach (var spellEntry in currentModel_.spells) 
		{
			EditorGUILayout.BeginHorizontal ();
			EditorHelper.FoldoutContent.text = "Spells for level "+spellEntry.maxEnemyLevel.ToString();
			spellEntry.expanded = EditorGUILayout.Foldout (spellEntry.expanded, EditorHelper.FoldoutContent);
			if (GUILayout.Button(EditorHelper.TrashCanIcon, EditorHelper.TrashCanButton))
			{
				entryToDelete = spellEntry;
			}
			EditorGUILayout.EndHorizontal ();


			if (spellEntry.expanded)
			{
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button("Add spell"))
				{
					spellEntry.spells.Add(new EnemySpell());
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField("Max enemy level");
				EditorGUILayout.Separator();
				spellEntry.maxEnemyLevel = EditorGUILayout.IntField(spellEntry.maxEnemyLevel);
				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel++;
				foreach(var spell in spellEntry.spells)
				{
					EditorGUILayout.BeginVertical();
					GUILayout.Box(spell.spellID, EditorHelper.Line);

					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField("ID");
					EditorGUILayout.Separator();
					var spellNames = SpellTab.Instance().SpellNames;
					spell.selectedSpell = EditorGUILayout.Popup(spell.selectedSpell, spellNames);
					if (spell.selectedSpell < spellNames.Length)
					{
						spell.spellID = spellNames[spell.selectedSpell];
					}
					if (GUILayout.Button(EditorHelper.TrashCanIcon, EditorHelper.TrashCanButton))
					{
						spellToDelete = spell;
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField("Weight");
					EditorGUILayout.Separator();
					spell.weight = EditorGUILayout.FloatField(spell.weight);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.EndVertical();
				}
				EditorGUI.indentLevel--;
				GUILayout.Box("", EditorHelper.Line);

				if (spellToDelete != null) 
				{
					spellEntry.spells.Remove(spellToDelete);
				}
			}
		}
		if (entryToDelete != null)
		{
			currentModel_.spells.Remove(entryToDelete);
		}
		EditorGUILayout.EndVertical ();
	}

	void DrawMonsterList()
	{
		EditorGUILayout.BeginVertical ();

		scrollPosition_ = EditorGUILayout.BeginScrollView (scrollPosition_, GUILayout.MaxWidth (400));

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
		var id = currentModel_.ID;
		currentModel_.ID = EditorGUILayout.TextField (currentModel_.ID);
		if (id != currentModel_.ID) 
		{
			RefreshEnemyNames();
		}
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
