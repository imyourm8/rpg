using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Attributes;

public class EffectTab
{
	private List<SpellEffectEntry> effects_;
	private SpellEffectEntry currentEffect_;
	private Vector2 scrollPosition_ = Vector2.zero;

	public SpellTab SpellTab;

	public EffectTab()
	{
		LootQuest.GameData.Effects.Instance.Load ();
		effects_ = LootQuest.GameData.Effects.Instance.Data;
	}

	public void GUI()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Save")) 
		{
			LootQuest.GameData.Effects.Instance.Save();
		}

		if (currentEffect_ != null && GUILayout.Button ("Remove Effect")) 
		{
			effects_.Remove(currentEffect_);
			currentEffect_ = null;
			
			SpellTab.RefreshEffectList();
		}

		if (GUILayout.Button ("Add Effect")) 
		{
			currentEffect_ = new SpellEffectEntry();
			effects_.Add(currentEffect_);

			SpellTab.RefreshEffectList();
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		DrawSpellList ();
		
		if (currentEffect_ != null)
			DrawNewSpellSection ();

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	void DrawSpellList ()
	{
		EditorGUILayout.BeginVertical ();
		
		scrollPosition_ = EditorGUILayout.BeginScrollView (scrollPosition_, GUILayout.Width (300));

		int count = effects_.Count;
		SpellEffectEntry nextModel = null;
		for (int i = 0; i < count; ++i) 
		{
			var effect = effects_[i];
			var title = effect.handler.ToString();
			title += " Lv:"+effect.level.ToString();
			if (GUILayout.Button (title, EditorHelper.ListSkin(effect==currentEffect_))) 
			{
				nextModel = effect;
			}
		}

		EditorGUILayout.EndScrollView ();

		if (nextModel != null) 
		{
			currentEffect_ = nextModel;
		}

		EditorGUILayout.EndVertical ();
	}
	
	void DrawNewSpellSection ()
	{
		bool refreshEffectList = false;

		EditorGUILayout.BeginVertical (GUILayout.Width (300));

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Title");
		EditorGUILayout.Separator ();
		currentEffect_.titleID = EditorGUILayout.TextField (currentEffect_.titleID);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();

		var currentEffID = currentEffect_.handler;
		currentEffect_.handler = (LootQuest.Game.Spells.SpellEffects.SpellEffectID)
			EditorGUILayout.EnumPopup ("Handler", currentEffect_.handler);

		refreshEffectList = currentEffID != currentEffect_.handler;

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Level");
		EditorGUILayout.Separator ();
		currentEffect_.level = EditorGUILayout.IntField (currentEffect_.level);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		currentEffect_.triggeredStatus = (LootQuest.Game.Status.EffectID)EditorGUILayout.EnumPopup ("Triggered status", currentEffect_.triggeredStatus);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Power");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentEffect_.effectPower);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Trigger chance");
		EditorGUILayout.Separator ();
		EditorHelper.DrawRange (currentEffect_.triggerChance);
		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.EndVertical ();

		if (refreshEffectList) 
		{
			SpellTab.RefreshEffectList();
		}
	}
}
