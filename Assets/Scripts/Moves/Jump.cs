using System;
using Moves;
using UnityEngine;

public class Jump : MonoBehaviour, Move {
	private bool _freezed;

	[SerializeField,
	Tooltip("The maximum force given to the jump of the player"),
	Range(1.0f, 10.0f)]
	private float _jumpForce;

	[SerializeField,
	Tooltip("The maximum force given to the double jump of the player"),
	Range(1.0f, 10.0f)]
	private float _doubleJumpForce;

	[SerializeField,
	Tooltip("The y velocity that the player can have when starting the double jump"),
	Range(.1f, 2f)]
	private float _maxDoubleJumpYVelocity;

	private bool canJumpAgain = true;

	private Rigidbody _rb;

	private void Start() {
		_rb = GetComponent<Rigidbody>();
		if (_rb == null) Debug.LogWarning("[Jump]::Start - RigidBody not found");
	}

	public void execute(bool isGrounded) {
		if (isGrounded) {
			canJumpAgain = true;
			_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
		} else if (canJumpAgain && Math.Abs(_rb.velocity.y) <= _maxDoubleJumpYVelocity) {
			_rb.AddForce(Vector3.up * _doubleJumpForce, ForceMode.Impulse);
			canJumpAgain = false;
		}
	}

	public bool Freezed { get => _freezed; set => _freezed = value; }

}