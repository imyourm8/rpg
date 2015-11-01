using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

[ExecuteInEditMode]
public class DamageNumber : MonoBehaviour 
{
	private Text text_;

	public delegate void DestroyCallback (DamageNumber script);
	public event DestroyCallback OnDestroy;

	private Vector2 position_;
	private float startTime_;
	public Vector2 scale;

	public Vector2 Scale
	{
		get { return scale; }
		set { scale = value; }
	}

	public void PlayRandom()
	{
		gameObject.transform.localPosition = Vector3.zero;
		//gameObject.transform.DOLocalMoveX(
	}
}
