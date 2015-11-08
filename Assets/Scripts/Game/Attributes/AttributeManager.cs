using UnityEngine;
using System.Collections.Generic;

namespace LootQuest.Game.Attributes
{
	public class AttributeManager
	{
		private Dictionary<AttributeID, Attribute> attributes_;

		public AttributeManager()
		{
			attributes_ = new Dictionary<AttributeID, Attribute> ();
		}

		public void Add(Attribute att)
		{
			if (attributes_.ContainsKey (att.ID))
				return;
			attributes_.Add (att.ID, att);
		}

		public Attribute Get(AttributeID id)
		{
			Attribute att = null;
			attributes_.TryGetValue (id, out att);
			return att;
		}

		public float GetFinValue(AttributeID id)
		{
			float value = 0.0f;
			Attribute att = Get (id);
			if (att != null) 
			{
				value = att.FinalValue;
			}
			return value;
		}

		public void SetValue(AttributeID id, float value)
		{
			Attribute att = Get (id);
			if (att != null) 
			{
				att.SetValue(value);
			}
		}

		public IEnumerator<Attribute> GetEnumerator()
		{
			return attributes_.Values.GetEnumerator();
		}
	}
}