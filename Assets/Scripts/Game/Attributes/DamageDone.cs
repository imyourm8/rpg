using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Attributes
{
	public class DamageDone : Attribute 
	{
		protected override float GetValue()
		{
			return base.GetValue ();
		}

        public override void CalculateValue()
        {
            base.CalculateValue();

            foreach (var att in attributes_)
            {
                if (att.ID == AttributeID.Damage)
                {
                    value_ += att.Value;
                }
            }
        }

        protected override float GetMultiplier()
        {
            var multFromSource = 0.0f;
            foreach (var att in attributes_)
            {
                if (att.ID == AttributeID.Damage)
                {
                    multFromSource += att.Multipler;
                }
            }
            return base.GetMultiplier() + multFromSource;
        }
	}
}