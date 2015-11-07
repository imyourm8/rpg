using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LootQuest.Game.Units;

namespace LootQuest.Game.Spells
{
    partial class Spell
    {
        private void HandleApplyWeaponDamage(SpellEffectData entry)
        {
			AttackEntry attack = new AttackEntry ();
			attack.damage = caster_.RollWeaponDamage();
            attack.damage *= (1.0f+entry.effectPower);
            //attack.type = AttackType.PHYSICAL;
            //attack.delay = entry.delay;
            attack.target = target_;
           // caster_.OnWeaponDamageApply(target_, attack);

			caster_.Attack (attack);
        }
		/*
        private void HandleApplySpellDamage(SpellEffects.SpellEffectEntry entry)
        {

        }

        private void HandleApplyStatusEffect(SpellEffects.SpellEffectEntry entry)
        {
            if (entry.triggeredStatusEffect == TapCommon.StatusEffects.NONE)
            {
                log.Error("Trying apply NONE status effect! Spell id: "+entry_.id.ToString());
                return;
            }

            var holders = target_.GetHolders(entry_);
            bool addNewEffect = true;
            StatusEffects.EffectHolder existedHolder = null;
            //find caster's spell 
            if (holders != null)
            {
                foreach (var holder in holders)
                {
                    if (holder.GetCasterGUID() == caster_.GUID)
                    {
                        existedHolder = holder;
                        //iterate over effects and compare stack amount
                        var effects = holder.GetEffects();
                        foreach (var eff in effects)
                        {
                            if (eff.Entry.effect == entry.triggeredStatusEffect)
                            {
                                if (eff.GetModel().Amount < entry_.maxStack)
                                {
                                    //still no max amount, increase it
                                    eff.GetModel().Amount++;
                                }
                                //refresh it anyway 
                                eff.Refresh();
                                addNewEffect = false;
                                break;
                            }

                        }
                    }
                }
            }

            if (addNewEffect)
            {
                //if there are no more spells can be applied on target
                if (holders != null && entry_.maxStacksOnTarget <= holders.Count)
                {
                    return;
                }

                if (existedHolder == null)
                {
                    existedHolder = new StatusEffects.EffectHolder(entry_, target_, caster_);
                    target_.Add(existedHolder);
                }
                var status = StatusEffects.Effect.Create(existedHolder, entry, target_.GetNextStatusEffectID());
                status.effectIndex = entry.index;
                existedHolder.Add(status);
            }
        }*/
    }
}
