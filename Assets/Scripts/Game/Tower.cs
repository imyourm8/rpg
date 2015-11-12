using UnityEngine;
using System.Collections;

namespace LootQuest.Game
{
    public class Tower
    {
        private GameData.TowerEntry entry_;
        private int level_;

        public GameData.TowerEntry Entry
        {
            get { return entry_; }
        }

        public int Level
        {
            get { return level_; }
        }

        public void Init(int level, GameData.TowerEntry entry)
        {
            level_ = level;
            entry_ = entry;
        }
    }
}