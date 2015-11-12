using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace LootQuest
{
    public class SpriteCollection : MonoBehaviour
    {
        public string spriteLabel;

        [SerializeField]
        private Utils.StringSpriteDict sprites = null;

        public Sprite GetSprite(string name)
        {
            if (name == null) return null;

            Sprite sprite = null;
            sprites.TryGetValue(name, out sprite);
            return sprite;
        }

        public void AddSprite(Sprite sprite)
        {
            sprites[sprite.name] = sprite;
        }

        public IEnumerator<KeyValuePair<string, Sprite>> GetEnumerator()
        {
            return sprites.GetEnumerator();
        }
    }

}