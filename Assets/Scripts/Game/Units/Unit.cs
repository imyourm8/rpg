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
		private CooldownManager<string> cooldowns_;
		protected LevelManager lvlManager_;

		public Unit()
		{
			cooldowns_ = new CooldownManager<string>(()=>Time.time);
			lvlManager_ = new LevelManager ();

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
			stats_.Add (dmgMin);
			stats_.Add (dmgMax);

			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.CritChance));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.CritDamage));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.HealthRegeneration));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.SpellDamage));
			stats_.Add (new LootQuest.Game.Attributes.Attribute ().Init(LootQuest.Game.Attributes.AttributeID.AttackRange));

			spells_ = new LootQuest.Game.Spells.SpellManager (this);
			spellSlots_ = new Dictionary<SlotStype, Spell> ();
		}

		public void LoadSpell(string id)
		{
			var spell = SpellExtensions.Create ();;
			spell.InitFromEntry (id, 1, this);
			spells_.Add (spell);
			PutSpellInSlot (spell.Data.entry.slot, spell);
		}

		public Spell GetSpell(SlotStype slot)
		{
			Spell spell;
			spellSlots_.TryGetValue (slot, out spell);
			return spell;
		}

		public bool PutSpellInSlot(SlotStype slot, Spell spell)
		{
			if (spell.Data.entry.slot != slot)
				return false;
			spellSlots_ [slot] = spell;
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
			//cooldowns_.Reset ();
		}

		public float GetDamageRoll()
		{
			return Random.Range(stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.DamageMin), stats_.GetFinValue(LootQuest.Game.Attributes.AttributeID.DamageMax));
		}

		public bool HasCooldown(string id)
		{
			return cooldowns_.HasCooldown(id);
		}

		public bool CanAutoAttack()
		{
			return !HasCooldown (autoAttackAbility_);
		}

		public float RollWeaponDamage()
		{
			float min = stats_.GetFinValue (LootQuest.Game.Attributes.AttributeID.DamageMin);
			float max = stats_.GetFinValue (LootQuest.Game.Attributes.AttributeID.DamageMax);
			return Random.Range (min, max);
		}

		public void PerformAutoAttack()
		{
			if (CanAutoAttack ()) 
			{
				var spell = spells_.GetSpellInSlot (LootQuest.Game.Spells.SlotStype.AutoAttack);
				if (spell != null)
					Cast (spell);
			}
		}

		private float GetAutoAttackCooldown()
		{
			float attSpeed = stats_.GetFinValue (LootQuest.Game.Attributes.AttributeID.AttackSpeed);
			return 1.0f / attSpeed;
		}

		private void Cast(Spell spell)
		{
			spell.Cast();
			cooldowns_.SetCooldown(spell.Data.entry.ID, GetAutoAttackCooldown());
		}

		public void Cast(string spell)
		{

		}

		public override bool CanAttack (Entity target)
		{
			return base.CanAttack (target);
		}

		public void Attack(AttackEntry entry)
		{
			var health = entry.target.Stats.Get (LootQuest.Game.Attributes.AttributeID.Health);
			health.ModifyValue (-entry.damage);

			Debug.LogFormat ("Damage done: {0} by {1} health left {2}", entry.damage, this.ToString (), health.FinalValue);

			if (health.FinalValue < 0)
				Debug.Log ("Target died");
		}

		public bool IsAlive()
		{
			return Stats.GetFinValue (LootQuest.Game.Attributes.AttributeID.Health) > 0.0f;
		}

		public override void Update ()
		{
			base.Update ();

			if (ai_ != null) 
			{
				ai_.Update();
			}
		}

		public override bool NeedToRemove ()
		{
			return !IsAlive();
		}
	}
}