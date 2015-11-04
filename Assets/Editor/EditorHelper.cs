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

	public static GUILayoutOption[] PlusButton;
	public static GUILayoutOption[] TrashCanButton;
	public static GUILayoutOption[] Line = new GUILayoutOption[]{GUILayout.ExpandWidth (true), GUILayout.Height (1)};

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
}
