using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles
{
	public static class ProjectileExtensions 
	{
		private static Utils.ObjectPool.ObjectPoolGeneric<Projectile> projectiles_ = new Utils.ObjectPool.ObjectPoolGeneric<Projectile> ();

		public static void Init()
		{
			projectiles_.Init (10, 0, false);
		}

		public static Projectile Create(Spells.SpellData spell)
		{
			var projectile = projectiles_.Get ();
			projectile.Init (spell);
			return projectile;
		}
		
		public static void Return(this Projectile projectile)
		{
			projectiles_.Return (projectile);
			projectile.Spell = null;
			projectile.View.gameObject.ReturnPooled ();
		}

		public static Behaviours.BaseBehaviour CreateBehaviour(Behaviours.BehaviourID id)
		{
			switch (id) 
			{
			case LootQuest.Game.Spells.Projectiles.Behaviours.BehaviourID.Dummy:
				return new Behaviours.BaseBehaviour();

			case LootQuest.Game.Spells.Projectiles.Behaviours.BehaviourID.StraightFly:
				return new Behaviours.StraightFly();

			case LootQuest.Game.Spells.Projectiles.Behaviours.BehaviourID.FlyToTarget:
				var behaviour = new Behaviours.StraightFly();
				behaviour.Behaviour = new Behaviours.FlyToTarget();
				return behaviour;
			}

			return null;
		}

		public static void Return(this Behaviours.BaseBehaviour behaviour)
		{
			behaviour.Projectile = null;
		}
	}
}