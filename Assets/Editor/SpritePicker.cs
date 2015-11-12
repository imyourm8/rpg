using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpritePicker : EditorWindow 
{
    public static void Show(LootQuest.SpriteCollection sprites, string sprite, int controlID)
    {
        var window = new SpritePicker(sprites, sprite, controlID);
        window.ShowUtility();
    }

    private LootQuest.SpriteCollection sprites_;
    private string sprite_;
    private Sprite selectedSprite_;
    private Sprite prevSprite_;
    private Vector2 scollPos_;
    private int controlID_;
    private int iconPickerControlID_ = -1;

    public SpritePicker(LootQuest.SpriteCollection sprites, string sprite, int controlID)
    {
        controlID_ = controlID;
        sprite_ = sprite;
        sprites_ = sprites;

        prevSprite_ = sprites_.GetSprite(sprite_);
        selectedSprite_ = prevSprite_;
		Event.current.commandName = "";
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(500));

        scollPos_ = EditorGUILayout.BeginScrollView(scollPos_);

        int h = 30;
        GUILayoutOption[] spr_param = new GUILayoutOption[] { GUILayout.Height(h), GUILayout.Width(h) };
        GUILayoutOption[] btn_param = new GUILayoutOption[] { GUILayout.Height(h), GUILayout.Width(40) };

        if (selectedSprite_ != null)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Selected");
            GUILayout.Label(AssetPreview.GetAssetPreview(selectedSprite_), spr_param);
            GUILayout.Label(selectedSprite_.name);
            EditorGUILayout.EndHorizontal();
        }

        foreach (var p  in sprites_)
        {
            Sprite spr = p.Value;
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label(AssetPreview.GetAssetPreview(spr), spr_param);
            GUILayout.Label(spr.name);
            if (GUILayout.Button("Show", btn_param))
            {
                EditorGUIUtility.PingObject(spr);
            }
            if (GUILayout.Button("Select", btn_param))
            {
                selectedSprite_ = spr;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        var currentControlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        if (GUILayout.Button("Add new"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(selectedSprite_, false, "", currentControlID);
        }
		
        string commandName = Event.current.commandName;
        if (commandName == "ObjectSelectorUpdated")
        {
            selectedSprite_ = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            this.Repaint();
        }
        else if (commandName == "ObjectSelectorClosed")
        {
            selectedSprite_ = (Sprite)EditorGUIUtility.GetObjectPickerObject();
        }

        if (selectedSprite_ != null)
        {
            sprites_.AddSprite(selectedSprite_);
        }

        EditorGUILayout.EndVertical();
    }

    void OnDestroy()
    {
		EditorHelper.SpritePickerPicked = selectedSprite_;

        if (prevSprite_ != selectedSprite_)
        {
            Event.current.commandName = "ObjectSelectorUpdated";
        }
    }

    void OnFocus()
    {
        GUIUtility.hotControl = controlID_;
    }
}
