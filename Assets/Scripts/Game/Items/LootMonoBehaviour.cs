using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Items
{
    public class LootMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Item item_;

        [SerializeField]
        private SpriteRenderer renderer_;

        [SerializeField]
        private SpriteCollection icons_;

        public void Init(Item item, string view)
        {
            item_ = item;
            renderer_.sprite = icons_.GetSprite(view);
        }
    }
}