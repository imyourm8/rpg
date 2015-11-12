using UnityEngine;
using System.Collections;

namespace LootQuest.Game.GameRules
{
    public class ExpGameRules : BaseRule
    {
        public override void OnEnemyKilled(Units.Enemy enemy, Units.Hero killer)
        {
            double exp = LevelManager.GetMobExpMultplier(killer.LevelManager.Level, enemy.LevelManager.Level);
            exp *= (1.0 + enemy.ExpGain);
            killer.LevelManager.AddExp(exp);
        }
    }
}