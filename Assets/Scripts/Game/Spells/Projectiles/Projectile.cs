using UnityEngine;

using System.Collections;

using LootQuest.Game;
using LootQuest.Game.Units;

namespace LootQuest.Game.Spells.Projectiles
{
	public class Projectile : Entity
	{
		private float radius_ = 0.0f;

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
			var viewPrefab = spell.entry.view.GetPooled ();
			View = viewPrefab.GetComponent<Game.Units.View.SpriteView> ();
			radius_ = viewPrefab.GetComponentsInChildren<CircleCollider2D> (true)[0].radius;
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

			this.Return ();
		}

		public override bool NeedToRemove ()
		{
			return remove_;
		}

		public override void Update ()
		{
			behaviour_.Update ();

			base.Update ();

			bool pierce = spell_.Data.entry.projectile.pierce;
			var targets = spell_.GetTargets ();
			int count = targets.Count;
			for (int i = 0; i < count; ++i) 
			{
				var target = targets[i] as Unit;

				if (target != null && game_.Distance(target, this) <= radius_)
				{
					spell_.ApplyEffectsOnTarget(target);
					Remove();
					if (!pierce) break;
				}
			}
		}
	}
}