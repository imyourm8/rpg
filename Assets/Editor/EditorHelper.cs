using UnityEngine;
using UnityEditor;

using LootQuest.Utils;

using System;
using System.Collections;

public class EditorHelper 
{
	static public Color tintColor = new Color(0.6f, 0.6f, 1.0f);

	static public Texture2D tableBGTexture;
	static public Texture2D tableHeaderBGTexture;
	static public Texture2D tintTex;
	static public Texture2D blackTex;
	
	static public GUIStyle tableStyle;
	static public GUIStyle tableHeaderStyle;
	static public GUIStyle tableFieldStyle;
	static public GUIStyle previewBackground;
	static public GUIStyle listStyle;
	static public GUIStyle listSelStyle;

	public void Init()
	{
		if (listStyle == null) {
			listStyle = new GUIStyle (GUI.skin.button);
			
			listStyle.alignment = TextAnchor.MiddleLeft;
			listStyle.normal.background = null;
			listStyle.active.background = null;
			listStyle.hover.background = null;
		}
		
		if (listSelStyle == null || tintTex == null) {
			tintTex = new Texture2D(1, 1);
			tintTex.SetPixel(0,0, tintColor);
			tintTex.Apply();
			
			listSelStyle = new GUIStyle (GUI.skin.button);
			listSelStyle.alignment = TextAnchor.MiddleLeft;
		}
		
		listSelStyle.normal.background = tintTex;
		listSelStyle.active.background = tintTex;
		listSelStyle.hover.background = tintTex;
	}

	public static void DrawTextfieldValue(LootQuest.Game.Attributes.Attribute att)
	{
		float value = 0.0f;
		try {
		value = float.Parse(EditorGUILayout.TextField (att.Value.ToString ()));
		} catch(FormatException e){}

		att.SetValue(value);
	}

	public static void DrawRange(LootQuest.GameData.AttributeTemplate att)
	{
		try {
			att.minValue = float.Parse(EditorGUILayout.TextField (att.minValue.ToString ()));
			att.maxValue = float.Parse(EditorGUILayout.TextField (att.maxValue.ToString ()));
		} catch(FormatException e){}
	}

	public static void DrawRange(Range<float> range)
	{
		range.Min = EditorGUILayout.FloatField (range.Min);
		range.Max = EditorGUILayout.FloatField (range.Max);
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
