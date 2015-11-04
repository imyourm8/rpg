using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.Behavior
{
    enum BehaviorType
    { 
        CAST_ON_POSITION
    }

    abstract class BaseBehaviour : IDisposable
    {
        private Spell spell_;
        protected IDisposable action_;

        public BaseBehaviour()
        {
            
        }

        public Spell Spell
        {
            set { spell_ = value; }
        }

        public void OnCast()
        {
            //action_ = spell_.Caster.DelayAction(OnStart, spell_.Entry.castDelay);
        }

        public void Dispose()
        {
            if (action_ != null)
                action_.Dispose();
        }

        abstract protected void OnStart();
    }
}
