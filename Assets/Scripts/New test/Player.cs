using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private CharacterController _controller;

	#region Movement variables
	[SerializeField]
	private float _speed = 5.0f;
	[SerializeField]
	private float _jumpHeight = 10.0f;
	#endregion

	#region Jump variables
	[SerializeField]
	private float _gravity = 1.0f;
	[SerializeField]
	private float _maxDoubleJumpVelocity = 5.0f;
	private float _yVelocity = 0.0f;
	private bool _jumpInput = false;
	private bool _canDoubleJump = false;
	#endregion

	#region Crouch variables
	[SerializeField]
	private float _crouchResize = 1.0f;

	[SerializeField]
	private float _crouchDuration = 1.0f;

	private float _originalControllerheight;
	private bool _canCrouch = true;
	#endregion



	// Start is called before the first frame update
	void Start() {
		_controller = GetComponent<CharacterController>();
		#region Events subscription
		// Subscribe to swiping functionalities
		// SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		// SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		// SwipeController.Instance.onSwipeRight += OnSwipeRight;
		SwipeController.Instance.onTap += OnTap;
		#endregion
	}

	// Update is called once per frame
	void Update() {
		// * Specify direction to travel (Vector 3)
		Vector3 direction = Vector3.forward;
		// * Specify velocity to travel with (Vector 3) = direction * speed
		Vector3 velocity = direction * _speed;

		// * Check for jumping
		if (_controller.isGrounded) {
			if (_jumpInput) {
				_yVelocity = _jumpHeight;
				_jumpInput = false;
			}
		} else {
			// * Add gravity
			_yVelocity -= _gravity;
			if (Math.Abs(_yVelocity) <= _maxDoubleJumpVelocity) {
				_canDoubleJump = true;
			}

			if (_canDoubleJump && _jumpInput) {
				_yVelocity += _jumpHeight;
				_jumpInput = false;
				_canDoubleJump = false;
			}
		}

		velocity.y = _yVelocity;
		// * Move based on velocity * timeDelta
		_controller.Move(velocity * Time.deltaTime);
	}

	private void OnTap() {
		_jumpInput = true;
	}

	private void OnSwipeDown() {
		if (_canCrouch) {
			_canCrouch = false;
			_originalControllerheight = _controller.height;
			_controller.height = _crouchResize;
			StartCoroutine("Crouch");
		}
	}

	private IEnumerator Crouch() {
		yield return new WaitForSeconds(_crouchDuration);
		_controller.height = _originalControllerheight;
		_canCrouch = true;
	}
}
