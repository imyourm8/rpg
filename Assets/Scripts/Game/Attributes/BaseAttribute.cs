using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Attributes
{
	public class BaseAttribute
	{
		protected float value_;
		protected float percentValue_;
		protected float multiplier_;
		protected AttributeID id_;

		public BaseAttribute()
		{
			value_ = 0.0f;
			multiplier_ = 0.0f;
			percentValue_ = 0.0f;
		}

		public void SetValue(float value)
		{
			value_ = value;
			CalculateValue ();
		}

		public void ModifyValue(float value)
		{
			value_ += value;
			CalculateValue ();
		}

		public AttributeID ID
		{ 
			get { return id_; }
		}

		public float Value
		{
			get { return value_; }
		}

		public float PercentValue
		{
			get { return percentValue_; }
		}

		public float Multipler
		{
			get { return multiplier_; }
		}

		public virtual void CalculateValue ()
		{}
	}
}