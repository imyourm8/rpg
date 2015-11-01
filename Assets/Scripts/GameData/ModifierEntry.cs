using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class ModifierEntry
	{
		public string id = "unique id";
		public string titleID = "title";

		public GameObject view = null;
		public GameObject projectileView = null;

		public Range<float> addRange = new Range<float>();
		public Range<float> range = new Range<float> ();
		public Range<float> addPower = new Range<float>();
		public Range<float> power = new Range<float> ();
		public Range<float> addChance = new Range<float>();
		public Range<float> chance = new Range<float>();
		public Range<int> addDuration = new Range<int>();
		public Range<int> duration = new Range<int>();
		public Range<float> addProjectileCount = new Range<float> ();
		public Range<int> projectileCount = new Range<int> ();
		public Range<float> projectileScale = new Range<float> ();
		public Range<int> addMaxStacks = new Range<int>();
		public Range<int> maxStacks = new Range<int>();

		public ModifierEntry()
		{}
	}
}