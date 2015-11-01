using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Spells;

namespace LootQuest.Game.Units 
{
	public class Unit : Entity
	{	
		private Game.Units.AI.BaseAI ai_;
		protected Spells.SpellManager spells_;
		protected string autoAttackAbility_;
		private Dictionary<SlotStype, Spell> spellSlots_;

		public Unit()
		{
			var AS = new LootQuest.Game.Attributes.AttackSpeed ().Init(LootQuest.Game.Attributes.AttributeID.AttackSpeed);
			var dex = new LootQuest.Game.Attributes.Attribute ().Init (LootQuest.Game.Attributes.AttributeID.Dexterity);
			AS.AddAttribute (dex);
			stats_.Add (dex);
			stats_.Add (AS);

			var str = new LootQuest.Game.Attributes.Attribute ().Init (LootQuest.Game.Attributes.AttributeID.Strength);
			var dmgMax = new LootQuest.Game.Attributes.DamageDone ().Init (LootQuest.Game.Attributes.AttributeID.DamageMax);
			var dmgMin = new LootQuest.Game.Attributes.DamageDone ().Init(LootQuest.Game.Attributes.AttributeID.DamageMin);

			dmgMin.AddAttribute (str);
			dmgMax.AddAttribute (str);

			stats_.Add (str);
			stats_.Add (dmgMax);
			stats_.Add (dmgMin);

			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.CritChance));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.CritDamage));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.HealthRegeneration));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.MovementSpeed));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.SpellDamage));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.AttackRange));

			spells_ = new LootQuest.Game.Spells.SpellManager (this);
			spellSlots_ = new Dictionary<SlotStype, Spell> ();
		}

		public void LoadSpell(string id)
		{
			var spell = new Spells.Spell ();
			spell.InitFromEntry (id, 1, this);
			spells_.Add (spell);
		}

		public bool PutSpellInSlot()
		{
			return true;
		}

		public Game.Units.AI.BaseAI AI
		{
			set { ai_ = value; ai_.Owner = this; }
			get { return ai_; }
		}

		public override void OnRemoveFromGame ()
		{
			base.OnRemoveFromGame ();
		}

		public float GetDamageRoll()
		{
			return Random.Range(stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.DamageMin), stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.DamageMax));
		}

		public bool HasCooldown(string id)
		{
			return true;
		}

		public bool CanAutoAttack()
		{
			return HasCooldown (autoAttackAbility_);
		}

		public void PerformAutoAttack()
		{
			if (CanAutoAttack ()) 
			{
				var spell = spells_.GetSpellInSlot(LootQuest.Game.Spells.SlotStype.AutoAttack);
				if (spell != null)
					spell.Cast();
			}
		}

		public void Cast(string spell)
		{

		}

		public override bool CanAttack (Entity target)
		{
			return base.CanAttack (target);
		}

		public override void Update ()
		{
			base.Update ();

			if (ai_ != null) 
			{
				ai_.Update();
			}
		}
	}
}