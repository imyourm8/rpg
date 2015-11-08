using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Attributes;

public class SpellTab
{
	private List<SpellEntry> spells_;
	private SpellEntry currentSpell_;
	private Vector2 scrollPosition_ = Vector2.zero;
	private Vector2 effectsScrollPosition_ = Vector2.zero;
	private int newEffect_ = 0;
	private string selectedEffect_;

	private HashSet<string> spellHash_;

	public HashSet<string> SpellHash;
	public HashSet<string> EffectsHash;
	public string[] SpellNames = new string[]{};
	public string[] EffectsNames = new string[]{};

	private static SpellTab instance_;
	public static SpellTab Instance()
	{
		return instance_;
	}

	public SpellTab()
	{
		instance_ = this;
		spellHash_ = new HashSet<string> ();
		SpellHash = spellHash_;
		EffectsHash = new HashSet<string> ();
		LootQuest.GameData.Spells.Instance.Load ();
		spells_ = LootQuest.GameData.Spells.Instance.Data;
		RefreshSpellNames ();
	}

	private void RefreshSpellNames()
	{
		spellHash_.Clear ();
		SpellNames = new string[spells_.Count];
		int count = spells_.Count;
		for(int i = 0; i < count; ++i)
		{
			SpellNames[i] = spells_[i].ID;
			spellHash_.Add(spells_[i].ID);
		}

		MobEditor.Instance ().RemoveSpells (spellHash_);
	}

	public void RefreshEffectList()
	{
		EffectsNames = new string[LootQuest.GameData.Effects.Instance.Data.Count];
		EffectsHash.Clear ();
		int i = 0;
		foreach (var eff in LootQuest.GameData.Effects.Instance.Data) 
		{
			EffectsNames [i++] = eff.id;
			EffectsHash.Add(eff.id);
		} 

		int count = spells_.Count;
		for (i = 0; i < count; ++i)
		{
			var spell = spells_[i];
			spell.effects.RemoveAll(x=>!EffectsHash.Contains(x));
		}
	}

	public void GUI()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Spells.Instance.Save();
		}

		if (currentSpell_ != null && GUILayout.Button ("Copy")) 
		{
			currentSpell_ = currentSpell_.Copy();
			spells_.Add(currentSpell_);
		}

		if (GUILayout.Button ("Check effects")) 
		{
			RefreshEffectList();
		}

		if (currentSpell_ != null && GUILayout.Button ("Remove Spell")) 
		{
			spells_.Remove(currentSpell_);
			currentSpell_ = null;
			RefreshSpellNames();
		}

		if (GUILayout.Button ("Add Spell")) 
		{
			currentSpell_ = new SpellEntry();
			spells_.Add(currentSpell_);
			RefreshSpellNames();
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		DrawSpellList ();
		
		if (currentSpell_ != null)
			DrawNewSpellSection ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	void DrawSpellList ()
	{
		EditorGUILayout.BeginVertical ();
		
		scrollPosition_ = EditorGUILayout.BeginScrollView (scrollPosition_, GUILayout.Width (300));

		int count = spells_.Count;
		SpellEntry nextModel = null;
		for (int i = 0; i < count; ++i) 
		{
			var spell = spells_[i];

			if (GUILayout.Button (spell.ID+" Lv:"+spell.level.ToString(), EditorHelper.ListSkin(spell==currentSpell_))) 
			{
				nextModel = spell;
			}
		}

		EditorGUILayout.EndScrollView ();

		if (nextModel != null) 
		{
			currentSpell_ = nextModel;
		}

		EditorGUILayout.EndVertical ();
	}
	
	void DrawNewSpellSection ()
	{
		EditorGUILayout.BeginVertical (GUILayout.Width (300));
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("ID");
		EditorGUILayout.Separator ();
		string prevID = currentSpell_.ID;
		currentSpell_.ID = EditorGUILayout.TextField (currentSpell_.ID);
		if (prevID != currentSpell_.ID) 
		{
			RefreshSpellNames();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Level", EditorStyles.label);
		EditorGUILayout.Separator ();
		currentSpell_.level = EditorGUILayout.IntField (currentSpell_.level);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Title");
		EditorGUILayout.Separator ();
		currentSpell_.titleID = EditorGUILayout.TextField (currentSpell_.titleID);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Slot Type");
		EditorGUILayout.Separator ();
		currentSpell_.slot = (LootQuest.Game.Spells.SlotStype)EditorGUILayout.EnumPopup ("", currentSpell_.slot);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Spell Type");
		EditorGUILayout.Separator ();
		currentSpell_.type = (LootQuest.Game.Spells.SpellType)EditorGUILayout.EnumPopup ("", currentSpell_.type);
		EditorGUILayout.EndHorizontal ();

		if (currentSpell_.type == LootQuest.Game.Spells.SpellType.Range) 
		{
			EditorGUILayout.LabelField ("Projectile Parameters");
			EditorGUI.indentLevel++;
			GUILayout.Box("", EditorHelper.Line);

			EditorGUILayout.BeginHorizontal ();
			currentSpell_.projectile.behaviour = (LootQuest.Game.Spells.Projectiles.Behaviours.BehaviourID)EditorGUILayout.EnumPopup ("Behaviour", currentSpell_.projectile.behaviour);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Speed", EditorStyles.label);
			EditorGUILayout.Separator ();
			currentSpell_.projectile.speed = EditorGUILayout.FloatField (currentSpell_.projectile.speed);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("View", EditorStyles.label);
			EditorGUILayout.Separator ();
			currentSpell_.projectile.view = (GameObject)EditorGUILayout.ObjectField (currentSpell_.projectile.view, typeof(GameObject), false);
			EditorGUILayout.EndHorizontal ();

			GUILayout.Box("", EditorHelper.Line);
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Target strategy");
		EditorGUILayout.Separator ();
		currentSpell_.targetStrategy = (LootQuest.Game.Spells.SpellTargetStrategy)EditorGUILayout.EnumPopup ("", currentSpell_.targetStrategy);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Target filter");
		EditorGUILayout.Separator ();
		currentSpell_.targetFilter = (LootQuest.Game.Spells.SpellTargetFilter)EditorGUILayout.EnumPopup ("", currentSpell_.targetFilter);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Cooldown", EditorStyles.label);
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange(currentSpell_.cooldown);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("View", EditorStyles.label);
		EditorGUILayout.Separator ();
		currentSpell_.view = (GameObject)EditorGUILayout.ObjectField (currentSpell_.view, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal ();

		#region Effects
		EditorGUILayout.BeginHorizontal ();

		if (newEffect_ < EffectsNames.Length)
		{

			if (GUILayout.Button ("Add")) 
			{
				currentSpell_.effects.Add(EffectsNames[newEffect_]);
			}
		}

		if (selectedEffect_ != null && GUILayout.Button ("Remove")) 
		{
			currentSpell_.effects.Remove(selectedEffect_);
			selectedEffect_ = null;
		}

		newEffect_ = EditorGUILayout.Popup (newEffect_, EffectsNames);
		
		EditorGUILayout.EndHorizontal ();

		if (newEffect_ < EffectsNames.Length)
		{
			effectsScrollPosition_ = EditorGUILayout.BeginScrollView (effectsScrollPosition_);
			
			int count = currentSpell_.effects.Count;
			string selectedEffect = EffectsNames[newEffect_];
			for (int i = 0; i < count; ++i) 
			{
				var eff = currentSpell_.effects[i];
				if (GUILayout.Button(eff, EditorHelper.ListSkin(selectedEffect==eff)))
				{
					selectedEffect_ = eff;
				}
			}
			EditorGUILayout.EndScrollView ();
		}

		#endregion
		EditorGUILayout.EndVertical ();
	}
}
