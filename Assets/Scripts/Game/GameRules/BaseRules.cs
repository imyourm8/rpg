using UnityEngine;
using System.Collections;

namespace LootQuest.Game.GameRules
{
    public class BaseRule
    {
        public virtual void OnEnemyKilled(Units.Enemy enemy, Units.Hero killer)
        { }
    }
}