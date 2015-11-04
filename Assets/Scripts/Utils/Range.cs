using UnityEngine;
using System.Collections;

namespace LootQuest.Utils 
{
	public class Range<T>
	{
		private T min_, max_;
		public Range()
		{

		}

		public Range<T> Copy()
		{
			var r = new Range<T> (min_, max_);
			return r;
		}

		public Range(T min, T max)
		{
			min_ = min;
			max_ = max;
		}
		
		public T Min 
		{
			get { return min_; }
			set { min_ = value; }
		}
		
		public T Max 
		{
			get { return max_; }
			set { max_ = value; }
		}
	}

	public static class RangeUtils
	{
		public static void FromJson(Range<float> range, JSONObject obj)
		{
			range.Min = obj ["min"].f;
			range.Max = obj ["max"].f;
		}
		
		public static JSONObject ToJson(Range<float> range)
		{
			JSONObject obj = new JSONObject ();
			obj.AddField("min", range.Min);
			obj.AddField("max", range.Max);
			return obj;
		}

		public static void FromJson(Range<int> range, JSONObject obj)
		{
			range.Min = (int)obj ["min"].i;
			range.Max = (int)obj ["max"].i;
		}
		
		public static JSONObject ToJson(Range<int> range)
		{
			JSONObject obj = new JSONObject ();
			obj.AddField("min", range.Min);
			obj.AddField("max", range.Max);
			return obj;
		}
	}
}