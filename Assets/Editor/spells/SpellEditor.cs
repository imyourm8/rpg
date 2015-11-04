using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.GameData;
using LootQuest.Game.Attributes;

public class SpellEditor : ConfigurationEditor.IEditorTab 
{
	private int tabIndex_ = 0;
	private string[] tabsTitles_ = new string[]{"Spells","Effects","Modifiers"};
	private SpellTab spells_ = new SpellTab();
	private EffectTab effects_ = new EffectTab ();
	private ModifierTab modifers_ = new ModifierTab();

	public SpellEditor()
	{
		effects_.SpellTab = spells_;
	}

	public string Title 
	{
		get { return "Spells"; }
	}

	public void OnGUI ()
	{
		tabIndex_ = GUILayout.Toolbar (tabIndex_, tabsTitles_);
		
		switch (tabIndex_) 
		{
		case 0:
			spells_.GUI ();
			break;
		case 1:
			effects_.GUI ();
			break;
		case 2:
			modifers_.GUI ();
			break;
		}
	}
}