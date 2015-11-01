using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Attributes;

public class ModifierTab
{
	private List<ModifierEntry> mods_;
	private ModifierEntry currentMod_;
	private Vector2 scrollPosition_ = Vector2.zero;

	public ModifierTab()
	{
		LootQuest.GameData.Modifiers.Instance.Load ();
		mods_ = LootQuest.GameData.Modifiers.Instance.Data;
	}

	public void GUI()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Modifiers.Instance.Save();
		}

		if (GUILayout.Button ("Add Mod")) 
		{
			currentMod_ = new ModifierEntry();
			mods_.Add(currentMod_);
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		DrawModifierList ();
		
		if (currentMod_ != null)
			DrawNewModifierSection ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	void DrawModifierList ()
	{
		EditorGUILayout.BeginVertical ();
		
		scrollPosition_ = EditorGUILayout.BeginScrollView (scrollPosition_, GUILayout.Width (300));

		int count = mods_.Count;
		ModifierEntry nextModel = null;
		for (int i = 0; i < count; ++i) 
		{
			var mod = mods_[i];

			if (GUILayout.Button (mod.id, EditorHelper.ListSkin(mod==currentMod_))) 
			{
				nextModel = mod;
			}
		}

		EditorGUILayout.EndScrollView ();

		if (nextModel != null) 
		{
			currentMod_ = nextModel;
		}

		EditorGUILayout.EndVertical ();
	}
	
	void DrawNewModifierSection ()
	{
		EditorGUILayout.BeginVertical (GUILayout.Width (300));

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("ID");
		EditorGUILayout.Separator ();
		currentMod_.id = EditorGUILayout.TextField (currentMod_.id);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Title");
		EditorGUILayout.Separator ();
		currentMod_.titleID = EditorGUILayout.TextField (currentMod_.titleID);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Prefab");
		EditorGUILayout.Separator ();
		currentMod_.view = (GameObject)EditorGUILayout.ObjectField (currentMod_.view, typeof(GameObject));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Duration");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.duration);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Power");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.power);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Projectile Count");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.projectileCount);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Projectile prefab");
		EditorGUILayout.Separator ();
		currentMod_.projectileView = (GameObject)EditorGUILayout.ObjectField (currentMod_.projectileView, typeof(GameObject));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Projectile scale");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.projectileScale);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Max stacks");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.maxStacks);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Range");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentMod_.range);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}
}
