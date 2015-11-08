using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.Collections.Generic;

public static class Extensions
{
	public static Vector2 Scale(this Vector2 vec, float value)
	{
		vec.x *= value;
		vec.y *= value;
		return vec;
	}

	public static float Randomize(this LootQuest.Utils.Range<float> rang)
	{
		return UnityEngine.Random.Range (rang.Min, rang.Max);
	}
	
	public static int Randomize(this LootQuest.Utils.Range<int> rang)
	{
		return UnityEngine.Random.Range (rang.Min, rang.Max);
	}

	public static string GetPrefabPath(this GameObject prefab)
	{
		//var parentObject = EditorUtility.GetPrefabParent(prefab); 
		if (prefab == null)
			return "";
		return AssetDatabase.GetAssetPath(prefab);
	}

	public static void AddChild(this GameObject parent, GameObject child)
	{
		child.transform.SetParent(parent.transform, false);
	}

	public static void Detach(this GameObject child)
	{
		child.transform.SetParent (null, false);
	}

	private static List<GameObject> buffer = new List<GameObject> ();

	public static void DetachChildren(this GameObject obj)
	{
		foreach(Transform child in obj.transform)
		{
			buffer.Add(child.gameObject);
		}

		int c = buffer.Count;
		for (int i = 0; i < c; ++i) 
		{
			buffer[c].transform.SetParent(null, false);
		}
	}

	public static void Resize<T>(this List<T> list, int sz, T c)
	{
		int cur = list.Count;
		if(sz < cur)
			list.RemoveRange(sz, cur - sz);
		else if(sz > cur)
		{
			if(sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
				list.Capacity = sz;
			list.AddRange(Enumerable.Repeat(c, sz - cur));
		}
	}
	public static void Resize<T>(this List<T> list, int sz) where T : new()
	{
		Resize(list, sz, new T());
	}
}
