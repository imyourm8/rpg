using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.Game.Items;
using LootQuest.Game.Finance;
using LootQuest.GameData;

public class ItemEditor : ConfigurationEditor.IEditorTab
{
	string ConfigurationEditor.IEditorTab.Title {
		get {
			return "Items";
		}
	}

    private List<ItemEntry> items_;
    private LootQuest.GameData.ItemEntry item_;
    private Vector2 scrollPosition_;
    private SpriteCollection itemSprites_;

    public static string[] ItemNames = new string[0];

    public ItemEditor()
    { 
        itemSprites_ = Resources.Load<GameObject>("Prefabs/SpriteCollections/ItemIconsCollection").GetComponent<SpriteCollection>();
        LootQuest.GameData.Items.Instance.Load();
        items_ = LootQuest.GameData.Items.Instance.Data;
        RefreshItems();
    }

    private void CheckItems()
    {}

    private void RefreshItems()
    {
        ItemNames = new string[items_.Count];

        int i = 0;
        foreach (var item in items_)
        {
            ItemNames[i++] = item.id;
        }
    }

	void ConfigurationEditor.IEditorTab.OnGUI ()
	{
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            LootQuest.GameData.Items.Instance.Save();
        }

        if (item_ != null && GUILayout.Button("Remove Item"))
        {
            items_.Remove(item_);
            item_ = null;
            RefreshItems();
        }

        if (GUILayout.Button("Check"))
        {
            CheckItems();
        }

        if (GUILayout.Button("Add Item"))
        {
            item_ = new ItemEntry();
            items_.Add(item_);
            RefreshItems();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        DrawItemList();

        if (item_ != null)
        {
            DrawItem();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
	}

    private void DrawItemList()
    {
        scrollPosition_ = EditorGUILayout.BeginScrollView(scrollPosition_, GUILayout.MaxWidth(400));

        int total = items_.Count;
        ItemEntry nextModel = null;
        for (int i = 0; i < total; ++i)
        {
            var model = items_[i];
            if (GUILayout.Button(model.id, EditorHelper.ListSkin(model == item_)))
            {
                nextModel = model;
            }
        }

        if (nextModel != null)
        {
            item_ = nextModel;
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawItem()
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(400));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID");
        EditorGUILayout.Separator();
        var prevID = item_.id;
        item_.id = EditorGUILayout.TextField(item_.id);
        if (prevID != item_.id) RefreshItems();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type");
        EditorGUILayout.Separator();
        item_.itemType = (ItemType)EditorGUILayout.EnumPopup(item_.itemType);
        EditorGUILayout.EndHorizontal();

        if (item_.itemType == ItemType.Currency)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Currency");
            EditorGUILayout.Separator();
            item_.currency = (CurrencyID)EditorGUILayout.EnumPopup(item_.currency);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal(); 
        item_.icon = EditorHelper.SpriteField("Icon", item_.icon, itemSprites_);
        EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal(); 
		item_.view = EditorHelper.SpriteField("View", item_.view, itemSprites_);
		EditorGUILayout.EndHorizontal();

        foreach (var att in item_.Stats)
        {
            EditorHelper.DrawRange(att);
        }

        EditorGUILayout.EndVertical();
    }
}
