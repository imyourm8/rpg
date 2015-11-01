using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class Extensions
{
	public static string GetPrefabPath(this GameObject prefab)
	{
		//var parentObject = EditorUtility.GetPrefabParent(prefab); 

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
}
