using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
	private Transform groundCheck;
	[SerializeField,
	Tooltip("The maximum distance from the ground when considering \"isGrounded\""),
	Range(0.0f, 1.0f)]
	private float groundDistance = 0.4f;

	[SerializeField]
	private LayerMask groundMask;
	private void Start() {
		groundCheck = gameObject.transform;
	}

	public bool isGrounded() {
		try {
			return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		} catch (Exception e) {
			Debug.LogError(e);
			return false;
		}
	}
}
