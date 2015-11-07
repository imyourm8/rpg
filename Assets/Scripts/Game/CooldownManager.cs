using System;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game
{
    public class CooldownManager<KeyType>
    {
		private Dictionary<KeyType, float> cooldowns_ = new Dictionary<KeyType, float>();
        private Func<float> time_;

		public CooldownManager(Func<float> timeFunc)
        {
            time_ = timeFunc;
        }

		public void SetCooldown(KeyType key, float cooldown)
        {
            float cd = time_() + cooldown;
            if (cooldowns_.ContainsKey(key))
            {
                cooldowns_[key] = cd;
            }
            else
            {
                cooldowns_.Add(key, cd);
            }
        }

        public bool HasCooldown(KeyType key)
        {
            return cooldowns_.ContainsKey(key) && cooldowns_[key] - time_() > 0;
        }

        public float GetCooldown(KeyType key)
        {
            if (!cooldowns_.ContainsKey(key))
            {
                return 0;
            }
            return Math.Max(cooldowns_[key] - time_(), 0);
        }

        public void Remove(KeyType key)
        {
            cooldowns_.Remove(key);
        }

        public void Reset()
        {
            cooldowns_.Clear();
        }
    }
}
