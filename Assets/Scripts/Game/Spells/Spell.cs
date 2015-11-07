using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Collections;

using Retlang.Fibers;

using LootQuest.Game.Spells.SpellEffects;
using LootQuest.Game.Spells.TargetChooseStrategy;
using LootQuest.Game.Units;

namespace LootQuest.Game.Spells
{
    public partial class Spell : 
        IDisposable, 
		TargetChooseStrategy.ITargetFilter
    {
        //static private ILogger log = LogManager.GetCurrentClassLogger();
        private SpellData data_;
        private Game.Units.Unit caster_;
		private Game.Units.Unit target_;
        private Behavior.BaseBehaviour behaviour_;
        private float castPosition_ = 0;
		private List<Game.Units.Entity> targets_;
        private int level_;
		private Dictionary<SpellEffectID, Action<SpellEffectData>> spellEffects_;

        private static Dictionary<SpellTargetStrategy, ITargetStrategy> strategies_
            = new Dictionary<SpellTargetStrategy, ITargetStrategy>();

		private delegate bool TargetFilter(Entity target);
        private TargetFilter targetFilter_;

        public static void Init()
        {
            //Add target strats
            strategies_.Add(SpellTargetStrategy.NEAREST, new ChooseFirstN(new ByDistance(new TargetStrategy())));
            strategies_.Add(SpellTargetStrategy.IN_RADIUS, new TargetStrategy());
			strategies_.Add(SpellTargetStrategy.RANDOM, new TargetStrategy());
        }

		public Spell()
		{
			spellEffects_ = new Dictionary<SpellEffectID, Action<SpellEffectData>> ();

			spellEffects_.Add (SpellEffectID.APPLY_DAMAGE, HandleApplyWeaponDamage);
		}

        public void Dispose()
        {
            if (behaviour_ != null)
            {
                behaviour_.Dispose();
            }

            caster_ = null;
            data_ = null;
        }

        public Game.Units.Unit Caster
        {
            get { return caster_; }
        }

		public SpellData Data
        { 
            get { return data_; } 
        }

        public PreparationResult Prepare()
        {
            if (caster_.HasCooldown(data_.entry.ID))
            {
                return PreparationResult.NOT_READY;
            }

            return PreparationResult.OK;
        }

        public void Cast()
        {
            if (behaviour_ != null)
                behaviour_.OnCast();

			DoSpellActions ();
        }

        private void DoSpellActions()
        {
            //TODO use following functions, to determine cast position
            castPosition_ = caster_.X;
            //choose target by strategy ID and unit mask

			var strat = strategies_[data_.entry.targetStrategy];
			targets_ = strat.FilterTargets(caster_.Game, this);

			switch (data_.entry.type) 
			{
			case SpellType.Melee:
				HandleMeleeCast();
				break;
			}

            //caster_.OnSpellActions(this);
        }

		private void ApplyEffectsOnTargets()
		{
			//apply effects on targets
			foreach (var target in targets_)
			{
				target_ = target as Unit;
				ApplyEffects(target_);
			}
		}

        public void InitFromEntry(string id, int level, Game.Units.Unit owner)
        {
            level_ = level;
			data_ = SpellGenerator.Instance.Generate (id);
            caster_ = owner;

            castPosition_ = caster_.X;
            switch (data_.entry.targetFilter)
            {
                case SpellTargetFilter.ENEMY:
                    targetFilter_ = EnemyTargetFilter;
                    break;
                case SpellTargetFilter.FRIENDLY:
                    targetFilter_ = FriendlyTargetFilter;
                    break;
                case SpellTargetFilter.ALL:
                    targetFilter_ = AllTargetFilter;
                    break;
                case SpellTargetFilter.CASTER:
                    targetFilter_ = CasterTargetFilter;
                    break;
            }
        }

		private bool EnemyTargetFilter(Entity target)
        {
            Unit unitTarget = target as Unit;
            bool isAttackable = unitTarget != null && caster_.CanAttack(unitTarget);
            return isAttackable; 
        }

		private bool FriendlyTargetFilter(Entity target)
        {
            Unit unitTarget = target as Unit;
            bool isAttackable = unitTarget != null &&
                (unitTarget != caster_ && !caster_.CanAttack(unitTarget));
            return !isAttackable;
        }

		private bool CasterTargetFilter(Entity target)
        {
            Unit unitTarget = target as Unit;
            return unitTarget != null && unitTarget == caster_;
        }

		private bool AllTargetFilter(Entity target)
        {
            return true;
        }

		private void ApplyEffects(Unit target)
        {
			int count = data_.effects.Count;
			for (int i = 0; i < count; ++i)
			{
				var effect = data_.effects[i];
				spellEffects_[effect.entry.handler](effect);
			}
        }

        bool TargetChooseStrategy.ITargetFilter.Filter(Entity target)
        {
            bool customConditions = true;
            return targetFilter_(target) && customConditions;
        }

        int TargetChooseStrategy.ITargetFilter.TargetsMaxCount()
        {
            return 1;
        }

        float TargetChooseStrategy.ITargetFilter.DistanceTo(Entity target)
        {
			return caster_.Game.Distance (caster_, target);
        }
    }
}
