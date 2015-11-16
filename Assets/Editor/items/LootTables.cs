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
    private List<LootTableEntry> filteredTables_;
    private List<string> tableGroups_;
    private string selectedGroup_;
    private Vector2 scrollPos_;
    private Vector2 groupScrollPos_;
    private Vector2 dropScrollPos_;

    public static string[] TableNames;

    public LootTables()
    {
        LootQuest.GameData.LootTables.Instance.Load();
        filteredTables_ = new List<LootTableEntry>();
        tableGroups_ = new List<string>();
        tables_ = LootQuest.GameData.LootTables.Instance.Data;

        HashSet<string> uniqueGroups = new HashSet<string>();
        foreach (var table in tables_)
        {
            uniqueGroups.Add(table.group);
        }

        foreach (var group in uniqueGroups)
        {
            tableGroups_.Add(group);
        }

        RefreshTableNames();
    }

    void RefreshTableNames()
    {
        TableNames = new string[tables_.Count];
        int count = tables_.Count;
        for (int i = 0; i < count; ++i)
        {
            TableNames[i] = tables_[i].id;
        }
    }

    void DrawTopMenu()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            LootQuest.GameData.LootTables.Instance.Save();
        }
        

        if (GUILayout.Button("Check"))
        {
            //CheckItems();
        }

        if (GUILayout.Button("Add Group"))
        {
            selectedGroup_ = "<unique_id>";
            tableGroups_.Add(selectedGroup_);
        }

        if (selectedGroup_ != null && GUILayout.Button("Remove Group"))
        {
            tableGroups_.Remove(selectedGroup_);
            tables_.RemoveAll(x => x.group == selectedGroup_);
            selectedGroup_ = null;
        }

        if (selectedGroup_ != null)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (GUILayout.Button("Add Table"))
            {
                table_ = new LootTableEntry();
                table_.group = selectedGroup_;
                tables_.Add(table_);
                RefreshTableNames();
            }

            if (table_ != null && GUILayout.Button("Remove Table"))
            {
                tables_.Remove(table_);
                table_ = null;
                RefreshTableNames();
            }

            if (table_ != null && GUILayout.Button("Copy Table"))
            {
                table_ = new LootTableEntry(table_);
                tables_.Add(table_);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

	void ConfigurationEditor.IEditorTab.OnGUI () 
	{
        EditorGUILayout.BeginVertical();

        DrawTopMenu();

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        DrawTableGroups();

        if (selectedGroup_ != null)
        {
            DrawTableList();

            if (table_ != null)
            {
                DrawTable();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
	}

    void DrawTableGroups()
    {
        groupScrollPos_ = EditorGUILayout.BeginScrollView(groupScrollPos_, GUILayout.MaxWidth(400));

        string nextGroup = null;
        int total = tableGroups_.Count;
        for (int i = 0; i < total; ++i)
        {
            string group = tableGroups_[i];
            if (selectedGroup_ == group)
            {
                var prevSelectedGroup = selectedGroup_;
                selectedGroup_ = EditorGUILayout.TextField(selectedGroup_);
                if (prevSelectedGroup != selectedGroup_)
                {
                    tables_.FindAll(x => x.group == prevSelectedGroup).ForEach(x => x.group = selectedGroup_);
                    tableGroups_[tableGroups_.IndexOf(prevSelectedGroup)] = selectedGroup_;
                }
            }
            else if (GUILayout.Button(group, EditorHelper.ListSkin(true)))
            {
                nextGroup = group;
            }
        }

        if (nextGroup != null)
            selectedGroup_ = nextGroup;

        EditorGUILayout.EndScrollView();
    }

    void DrawTableList()
    {
        scrollPos_ = EditorGUILayout.BeginScrollView(scrollPos_, GUILayout.MaxWidth(400));

        int total = tables_.Count;
        LootTableEntry nextModel = null;
        for (int i = 0; i < total; ++i)
        {
            var model = tables_[i];
            if (model.group != selectedGroup_) continue;
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
        var prevID = table_.id;
        table_.id = EditorGUILayout.TextField(table_.id);
        if (prevID != table_.id)
            RefreshTableNames();
        EditorGUILayout.LabelField(selectedGroup_);
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
