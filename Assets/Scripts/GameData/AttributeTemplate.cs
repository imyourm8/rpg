using UnityEngine;
using System.Collections;

namespace LootQuest.GameData
{
	public class AttributeTemplate
	{
		public Game.Attributes.AttributeID id;
		public float minValue = 0.0f;
		public float maxValue = 0.0f;

		public AttributeTemplate(Game.Attributes.AttributeID attID)
		{
			id = attID;
		}

		public void Generate(Game.Attributes.Attribute att)
		{
			att.SetValue (Random.Range (minValue, maxValue));
		}
	}
}