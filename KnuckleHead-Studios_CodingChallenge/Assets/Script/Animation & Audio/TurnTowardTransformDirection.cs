using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
	//This script rotates an object toward the 'forward' direction of another target transform;
	public class TurnTowardTransformDirection : MonoBehaviour {

		public Transform targetTransform;
		private Transform parentTransform;

		//Setup;
		void Start () {
			parentTransform = transform.parent;

			if(targetTransform == null)
				Debug.LogWarning("No target transform has been assigned to this script.", this);
		}
		
		//Update;
		void LateUpdate () {

			if(!targetTransform)
				return;

			//Calculate up and forward direction;
			Vector3 _forwardDirection = Vector3.ProjectOnPlane(targetTransform.forward, parentTransform.up).normalized;
			Vector3 _upDirection = parentTransform.up;

			//Set rotation;
			transform.rotation = Quaternion.LookRotation(_forwardDirection, _upDirection);
		}
	}
}
