using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using LootQuest.GameData;

public class GenerationRules : ConfigurationEditor.IEditorTab 
{
    string ConfigurationEditor.IEditorTab.Title
    {
        get { return "Gen. Rules"; }
    }

    private List<ItemGenerationRulesEntry> rules_;

    public GenerationRules()
    {
        ItemGenerationRules.Instance.Load();
        rules_ = ItemGenerationRules.Instance.Data;
    }

    void ConfigurationEditor.IEditorTab.OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            ItemGenerationRules.Instance.Save();
        }

        EditorGUILayout.EndHorizontal();

        DrawRules();

        EditorGUILayout.EndVertical();
    }

    void DrawRules()
    {
 
    }
}
