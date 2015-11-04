using UnityEngine;
using UnityEditor;

using System;
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
	private int newEffect_;

	public ModifierTab()
	{
		LootQuest.GameData.Modifiers.Instance.Load ();
		mods_ = LootQuest.GameData.Modifiers.Instance.Data;
	}

	public void RefreshEffectList()
	{
		SpellTab.Instance().RefreshEffectList ();
		int count = mods_.Count;
		for (int i = 0; i < count; ++i)
		{
			var mod = mods_[i];
			if (!SpellTab.Instance().EffectsHash.Contains(mod.spellEffect))
			{
				mod.spellEffect = "";
			}
		}
	}

	public void GUI()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Modifiers.Instance.Save();
		}

		if (GUILayout.Button ("Check effects")) 
		{
			RefreshEffectList();
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

		EditorHelper.Draw ("Duration", currentMod_.duration);
		EditorHelper.Draw ("Add Duration", currentMod_.addDuration);

		EditorHelper.Draw ("Power", currentMod_.power);
		EditorHelper.Draw ("Add Power", currentMod_.addPower);

		EditorHelper.Draw ("Chance", currentMod_.chance);
		EditorHelper.Draw ("Add Chance", currentMod_.addChance);

		EditorHelper.Draw ("Projectiles Count", currentMod_.projectileCount);
		EditorHelper.Draw ("Add Projectiles Count", currentMod_.addProjectileCount);

		EditorHelper.Draw ("Projectile scale", currentMod_.projectileScale);
		EditorHelper.Draw ("Add Projectile scale", currentMod_.addProjectileScale);

		EditorHelper.Draw ("Effective Range", currentMod_.range);
		EditorHelper.Draw ("Add Effective Range", currentMod_.addRange);

		EditorHelper.Draw ("Max Stacks", currentMod_.maxStacks);
		EditorHelper.Draw ("Add Max Stacks", currentMod_.addMaxStacks);

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Projectile prefab");
		EditorGUILayout.Separator ();
		currentMod_.projectileView = (GameObject)EditorGUILayout.ObjectField (currentMod_.projectileView, typeof(GameObject));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Spell Effect");
		EditorGUILayout.Separator ();
		newEffect_ = EditorGUILayout.Popup (newEffect_, SpellTab.Instance().EffectsNames);
		currentMod_.spellEffect = SpellTab.Instance().EffectsNames [newEffect_];
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}
}
