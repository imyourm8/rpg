using UnityEngine;
using UnityEditor;

using LootQuest.Utils;

using System;
using System.Collections;

public class EditorHelper 
{
	static public Color tintColor = new Color(0.6f, 0.6f, 1.0f);

	public static Texture TrashCanIcon;
	public static Texture PlusIcon;
	public static GUIContent FoldoutContent = new GUIContent ();
    public static ConfigurationEditor window = null;
	public static GUILayoutOption[] PlusButton;
	public static GUILayoutOption[] TrashCanButton;
	public static GUILayoutOption[] Line = new GUILayoutOption[]{GUILayout.ExpandWidth (true), GUILayout.Height (1)};
    public static Sprite SpritePickerPicked = null;

	public static void Init()
	{
		TrashCanIcon = Resources.Load<Texture> ("Editor/trash-can");
		TrashCanButton = new GUILayoutOption[]{GUILayout.Width((float)TrashCanIcon.width), GUILayout.Height((float)TrashCanIcon.height)};

		PlusIcon = Resources.Load<Texture> ("Editor/plus");
		PlusButton = new GUILayoutOption[]{GUILayout.Width((float)PlusIcon.width), GUILayout.Height((float)PlusIcon.height)};
	}

	public static void DrawTextfieldValue(LootQuest.Game.Attributes.Attribute att)
	{
		att.SetValue(EditorGUILayout.FloatField (att.Value));
	}

	public static void DrawRange(LootQuest.GameData.AttributeTemplate att)
	{
		att.minValue = EditorGUILayout.FloatField (att.minValue);
		att.maxValue = EditorGUILayout.FloatField (att.maxValue);
	}

	public static void DrawRange(Range<float> range)
	{
		range.Min = EditorGUILayout.FloatField (range.Min);
		range.Max = EditorGUILayout.FloatField (range.Max);
	}

	public static void DrawRange(Range<int> range)
	{
		range.Min = EditorGUILayout.IntField (range.Min);
		range.Max = EditorGUILayout.IntField (range.Max);
	}

	public static void Draw(string label, Range<float> range)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (label);
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (range);
		EditorGUILayout.EndHorizontal ();
	}

	public static void Draw(string label, Range<int> range)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (label);
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (range);
		EditorGUILayout.EndHorizontal ();
	}

	public static float DrawTextfieldValue(float value)
	{
		try {
			value = float.Parse(EditorGUILayout.TextField (value.ToString ()));
		} catch(FormatException e){}

		return value;
	}

	public static long DrawTextfieldValue(long value)
	{
		try {
			value = long.Parse(EditorGUILayout.TextField (value.ToString ()));
		} catch(FormatException e){}
		
		return value;
	}

	public static GUIStyle ListSkin(bool val)
	{
		return val ? GUI.skin.box : GUI.skin.button;
	}

    static public string SpriteField(string label, string icon, LootQuest.SpriteCollection sprites)
    {
        if (sprites == null) return "";

        Sprite spr = sprites.GetSprite(icon);
        int height = 50, width = 0;
        EditorGUILayout.BeginHorizontal(width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width(width));

        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField(label, GUI.skin.label, width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width((width - height) * 0.3f));
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        EditorGUILayout.LabelField(spr == null ? "" : spr.name, GUI.skin.textField, width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width((width - height) * 0.7f));

        Rect rect = EditorGUILayout.BeginVertical(GUILayout.Height(height), GUILayout.Width(height));
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        EditorGUILayout.Separator();
        if (GUI.Button(new Rect(rect.x + rect.width - rect.height, rect.y, rect.height, rect.height), "", GUI.skin.box))
        {
            lastControl = controlID;
            SpritePicker.Show(sprites, icon, controlID);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

		float x = rect.x + rect.width - rect.height + 2;
		float y = rect.y + 2;
		float h = rect.height - 4;
		
		if (spr != null)
		{
			Texture t = spr.texture;
			Rect tr = spr.textureRect;
			
			float tx = x;
			float ty = y;
			float tw = tr.width;
			float th = tr.height;
			if (tw < th)
			{
				tw = h * tw / th;
				tx += (h - tw) / 2;
				th = h;
			}
			else if (tw > th)
			{
				th = h * th / tw;
				ty += (h - th) / 2;
				tw = h;
			}
			else
			{
				tw = h;
				th = h;
			}
			
			Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
			
			GUI.DrawTextureWithTexCoords(new Rect(tx, ty, tw, th), t, r);
		}

        if (lastControl == controlID)
        {
            string commandName = Event.current.commandName;
            spr = SpritePickerPicked;
            if (commandName == "ObjectSelectorUpdated")
            {
                window.Repaint();
            }
        }

        EditorGUILayout.EndHorizontal();

        return spr == null ? "" : spr.name;
    }

    static int lastControl;
    static public string IconField(string label, string icon, LootQuest.SpriteCollection sprites, string filter, int height = 100, int width = 0)
    {
        if (sprites == null) return ""; 
        Sprite spr = sprites.GetSprite(icon);

        EditorGUILayout.BeginHorizontal(width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width(width));

        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField(label, GUI.skin.label, width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width((width - height) * 0.3f));
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        EditorGUILayout.LabelField(spr == null ? "" : spr.name, GUI.skin.textField, width == 0 ? GUILayout.MinHeight(0) : GUILayout.Width((width - height) * 0.7f));

        Rect rect = EditorGUILayout.BeginVertical(GUILayout.Height(height), GUILayout.Width(height));
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        EditorGUILayout.Separator();
        if (GUI.Button(new Rect(rect.x + rect.width - rect.height, rect.y, rect.height, rect.height), "", GUI.skin.box))
        {
            lastControl = controlID;
            EditorGUIUtility.ShowObjectPicker<Sprite>(spr, false, filter, controlID);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        float x = rect.x + rect.width - rect.height + 2;
        float y = rect.y + 2;
        float h = rect.height - 4;

        if (spr != null)
        {
            Texture t = spr.texture;
            Rect tr = spr.textureRect;

            float tx = x;
            float ty = y;
            float tw = tr.width;
            float th = tr.height;
            if (tw < th)
            {
                tw = h * tw / th;
                tx += (h - tw) / 2;
                th = h;
            }
            else if (tw > th)
            {
                th = h * th / tw;
                ty += (h - th) / 2;
                tw = h;
            }
            else
            {
                tw = h;
                th = h;
            }

            Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

            GUI.DrawTextureWithTexCoords(new Rect(tx, ty, tw, th), t, r);
        }

        if (lastControl == controlID)
        {
            string commandName = Event.current.commandName;
            if (commandName == "ObjectSelectorUpdated")
            {
                spr = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                window.Repaint();
            }
            else if (commandName == "ObjectSelectorClosed")
            {
                spr = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            }

            if (spr != null)
                sprites.AddSprite(spr);
        }

        EditorGUILayout.EndHorizontal();

        return spr == null ? "" : spr.name;
    }
}
