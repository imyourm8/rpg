using UnityEngine;

using System.Collections;

using LootQuest.Game;
using LootQuest.Game.Units;

namespace LootQuest.Game.Spells.Projectiles
{
	public class Projectile : Entity
	{
		private Spell spell_;
		public Spell Spell
		{
			get { return spell_; }
			set { spell_ = value; }
		}

		private Spells.SpellEffectData effect_;
		public Spells.SpellEffectData Effect
		{
			set { effect_ = value; }
			get { return effect_; }
		}

		private float radius_;
		public float Radius
		{
			set { radius_ = value; }
			get { return radius_; }
		}

		private bool remove_ = false;
		public void Remove()
		{
			remove_ = true;
		}

		private Behaviours.BaseBehaviour behaviour_;
		public Behaviours.BaseBehaviour Behaviour 
		{
			set { behaviour_ = value; behaviour_.Projectile = this; }
		}

		public void Init(Game.Spells.SpellData spell)
		{
			remove_ = false;
			View = spell.entry.view.GetPooled ().GetComponent<Game.Units.View.SpriteView> ();
			stats_.Get (LootQuest.Game.Attributes.AttributeID.MovementSpeed).SetValue (spell.entry.projectile.speed);
			Behaviour = ProjectileExtensions.CreateBehaviour (spell.entry.projectile.behaviour);
		}

		public override void OnAddedToGame ()
		{
			base.OnAddedToGame ();

			if (behaviour_ != null) 
			{
				behaviour_.Prepare();
			}
		}

		public override void OnRemoveFromGame ()
		{
			base.OnRemoveFromGame ();

			if (behaviour_ != null) 
			{
				behaviour_.Return();
				behaviour_ = null;
			}
		}

		public override bool NeedToRemove ()
		{
			return remove_;
		}

		public override void Update ()
		{
			behaviour_.Update ();

			base.Update ();
		}
	}
}