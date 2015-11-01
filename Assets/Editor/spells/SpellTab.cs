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
	private LootQuest.Game.Spells.SpellEffects.SpellEffectID newEffect_;

	private HashSet<string> spellHash_;
	public string[] SpellNames = new string[]{};
	public int[] Effects = new int[]{};
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
		Effects = new int[LootQuest.GameData.Effects.Instance.Data.Count];
		EffectsNames = new string[LootQuest.GameData.Effects.Instance.Data.Count];

		int i = 0; 
		foreach(var eff in LootQuest.GameData.Effects.Instance.Data)
		{
			Effects[i++] = (int)eff.handler;
		}

		Array.Sort (Effects);

		i = 0;
		foreach (var eff in Effects) 
		{
			EffectsNames [i++] = ((LootQuest.Game.Spells.SpellEffects.SpellEffectID)eff).ToString();
		}

		int count = spells_.Count;
		for (i = 0; i < count; ++i)
		{
			var spell = spells_[i];
			spell.effects.RemoveAll(x=>Array.BinarySearch(Effects, (int)x)<0);
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

		#region Effects
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add")) 
		{
			currentSpell_.effects.Add(newEffect_);
		}
		
		if (GUILayout.Button ("Remove")) 
		{
			currentSpell_.effects.Remove(newEffect_);
		}
		
		newEffect_ = 
			(LootQuest.Game.Spells.SpellEffects.SpellEffectID)
				EditorGUILayout.IntPopup ((int)newEffect_, EffectsNames, Effects);
		
		EditorGUILayout.EndHorizontal ();

		effectsScrollPosition_ = EditorGUILayout.BeginScrollView (effectsScrollPosition_);
		
		int count = currentSpell_.effects.Count;
		for (int i = 0; i < count; ++i) 
		{
			var eff = currentSpell_.effects[i];
			if (GUILayout.Button(eff.ToString(), EditorHelper.ListSkin(newEffect_==eff)))
			{
				newEffect_ = eff;
			}
		}
		
		EditorGUILayout.EndScrollView ();
		#endregion
		EditorGUILayout.EndVertical ();
	}
}
