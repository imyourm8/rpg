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
        private GameData.SpellEntry entry_;
        private Game.Units.Unit caster_;
		private Game.Units.Unit target_;
        private Behavior.BaseBehaviour behaviour_;
        private float castPosition_ = 0;
		private List<Game.Units.Entity> targets_;
        private int level_;

        private static Dictionary<SpellTargetStrategy, ITargetStrategy> strategies_
            = new Dictionary<SpellTargetStrategy, ITargetStrategy>();

		private delegate bool TargetFilter(Entity target);
        private TargetFilter targetFilter_;

        public static void Init()
        {
            //Add target strats
            strategies_.Add(SpellTargetStrategy.NEAREST, new ChooseFirstN(new ByDistance(new TargetStrategy())));
            strategies_.Add(SpellTargetStrategy.IN_RADIUS, new TargetStrategy());
        }

        public void Dispose()
        {
            if (behaviour_ != null)
            {
                behaviour_.Dispose();
            }

            caster_ = null;
            entry_ = null;
        }

        public Game.Units.Unit Caster
        {
            get { return caster_; }
        }

        public GameData.SpellEntry Entry
        { 
            get { return entry_; } 
        }

        public PreparationResult Prepare()
        {
            if (caster_.HasCooldown(entry_.ID))
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

			var strat = strategies_[entry_.targetStrategy];
			targets_ = strat.FilterTargets(caster_.Game, this);

            switch (entry_.type) 
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
			entry_ = GameData.Spells.Instance.GetEntry (id);
            caster_ = owner;

            castPosition_ = caster_.X;
            switch (entry_.targetFilter)
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
