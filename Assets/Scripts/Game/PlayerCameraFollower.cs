using UnityEngine;
using System.Collections;

namespace LootQuest.Game 
{
	[RequireComponent(typeof(Camera))]
	public class PlayerCameraFollower : MonoBehaviour 
	{
		[SerializeField]
		private GameObject target;

		[SerializeField]
		private Vector3 velocity = Vector3.zero;

		[SerializeField]
		private Vector3 offset;

		[SerializeField]
		private float dampTime = 1.0f;

		[SerializeField]
		private float interpVelocity = 0.0f;

		private Camera camera_;
		private Vector3 targetPos_;
		private Vector3 prevTargetPosition_;
		private float z_;

		void Start()
		{
			camera_ = GetComponent<Camera> ();

			targetPos_ = transform.position;
			z_ = transform.position.z;
		}

		void Update()
		{
			return;
			Vector3 difference = prevTargetPosition_;
			difference -= target.transform.localPosition;
			difference += offset;

			camera_.transform.localPosition = Vector3.SmoothDamp(camera_.transform.localPosition, difference, ref velocity, dampTime);

			prevTargetPosition_ = target.transform.localPosition;
		}

		void FixedUpdate () {
			if (target)
			{

				Vector3 posNoZ = targetPos_;
				posNoZ.z = target.transform.position.z;
				
				Vector3 targetDirection = (target.transform.position - posNoZ);
				
				interpVelocity = targetDirection.magnitude * 5f;
				
				targetPos_ = posNoZ + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
				targetPos_.z = z_;

				transform.position = Vector3.Lerp( transform.position, targetPos_ + offset, 0.25f);
			}
		}

		public void SetTarget(GameObject t)
		{
			target = t;
			prevTargetPosition_ = target.transform.localPosition;
		}
	}
}