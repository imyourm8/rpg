using UnityEngine;
using System.Collections;

namespace LootQuest.Factory
{
	public class AttributeFactory : Utils.Singleton<AttributeFactory> 
	{
		public Game.Attributes.Attribute Create(Game.Attributes.AttributeID id)
		{
			Game.Attributes.Attribute att = null;

			switch (id) 
			{
			case Game.Attributes.AttributeID.AttackSpeed:
				break;
			default:
				att = new LootQuest.Game.Attributes.Attribute().Init(id);
				break;
			}

			return att;
		}
	}
}