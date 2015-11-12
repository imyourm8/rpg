using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Items;

public class LootTables : ConfigurationEditor.IEditorTab
{
	#region IEditorTab implementation

	string ConfigurationEditor.IEditorTab.Title {
		get {
			return "Loot Tables";
		}
	}

	#endregion

    private List<LootTableEntry> tables_;
    private LootTableEntry table_;
    private Vector2 scrollPos_;
    private Vector2 dropScrollPos_;

    public LootTables()
    {
        LootQuest.GameData.LootTables.Instance.Load();
        tables_ = LootQuest.GameData.LootTables.Instance.Data;
    }

	void ConfigurationEditor.IEditorTab.OnGUI () 
	{
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            LootQuest.GameData.LootTables.Instance.Save();
        }

        if (table_ != null && GUILayout.Button("Remove Table"))
        {
            tables_.Remove(table_);
            table_ = null;
        }

        if (GUILayout.Button("Check"))
        {
            //CheckItems();
        }

        if (GUILayout.Button("Add Table"))
        {
            table_ = new LootTableEntry();
            tables_.Add(table_);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        DrawTableList();

        if (table_ != null)
        {
            DrawTable();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
	}

    void DrawTableList()
    {
        scrollPos_ = EditorGUILayout.BeginScrollView(scrollPos_, GUILayout.MaxWidth(400));

        int total = tables_.Count;
        LootTableEntry nextModel = null;
        for (int i = 0; i < total; ++i)
        {
            var model = tables_[i];
            string id = model.id + " Level: " + model.level.Min.ToString() + "-" + model.level.Max.ToString(); 
            if (GUILayout.Button(id, EditorHelper.ListSkin(model == table_)))
            {
				nextModel = model;
			}
		}

        if (nextModel != null)
        {
            table_ = nextModel;
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawTable()
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(500));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID");
        EditorGUILayout.Separator();
        table_.id = EditorGUILayout.TextField(table_.id);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level");
        EditorHelper.DrawRange(table_.level);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Drop"))
        {
            LootTableItemDrop newDrop = new LootTableItemDrop();
            table_.items.Add(newDrop);
        }
        EditorGUILayout.EndHorizontal();

        foreach(var rarityDrop in table_.rarity)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(rarityDrop.rarity.ToString());
            EditorGUILayout.Separator();
            rarityDrop.weight = EditorGUILayout.IntField(rarityDrop.weight);
            EditorGUILayout.EndHorizontal();
        }


        dropScrollPos_ = EditorGUILayout.BeginScrollView(dropScrollPos_);
        LootTableItemDrop dropToDelete = null;
        EditorGUI.indentLevel++;
        foreach (var drop in table_.items)
        {
            GUILayout.Box("", EditorHelper.Line);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(EditorHelper.TrashCanIcon, EditorHelper.TrashCanButton))
            {
                dropToDelete = drop;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Weight");
            EditorGUILayout.Separator();
            drop.weight = EditorGUILayout.IntField(drop.weight);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Item Type");
            EditorGUILayout.Separator();
            drop.item = (ItemType)EditorGUILayout.EnumPopup(drop.item);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Table");
            EditorGUILayout.Separator();
            drop.table = EditorGUILayout.TextField(drop.table);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Level");
            EditorGUILayout.Separator();
            drop.level = EditorGUILayout.IntField(drop.level);
            EditorGUILayout.EndHorizontal();

            GUILayout.Box("", EditorHelper.Line);
        }
        if (dropToDelete != null)
            table_.items.Remove(dropToDelete);
        EditorGUI.indentLevel--;

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }
}
