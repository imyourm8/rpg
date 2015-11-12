using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpritePicker : EditorWindow 
{
    public static void Show(LootQuest.SpriteCollection sprites, string sprite)
    {
        var window = new SpritePicker(sprites, sprite);
        window.ShowUtility();
    }

    private LootQuest.SpriteCollection sprites_;
    private string sprite_;
    private Vector2 scollPos_;

    public SpritePicker(LootQuest.SpriteCollection sprites, string sprite)
    { 
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(500));

        EditorGUILayout.BeginScrollView(scollPos_);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }
}
