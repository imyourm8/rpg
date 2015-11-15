using UnityEngine;
using System.Collections;

namespace LootQuest.GameData
{
	public class AttributeTemplate
	{
		public Game.Attributes.AttributeID id;
        public LootQuest.Utils.Range<float> value = new Utils.Range<float>();
        public LootQuest.Utils.Range<float> percentValue = new Utils.Range<float>();

        //for item generation
        public int weight = 0;
        public int accWeight = 0;

        public bool expanded = false;

		public AttributeTemplate(Game.Attributes.AttributeID attID)
		{
			id = attID;
		}

		public void Generate(Game.Attributes.Attribute att)
		{
			att.SetValue (value.Randomize());
            att.SetPercentValue(percentValue.Randomize());
		}
	}
}