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
	private bool _canAirMove = false;
	#endregion

	#region Crouch variables
	[SerializeField]
	private float _crouchResize = 1.0f;

	[SerializeField]
	private float _crouchDuration = 1.0f;

	private float _originalControllerheight;
	private bool _canCrouch = true;
	#endregion

	#region Dash variables
	[SerializeField]
	private float _dashDuration = 1.0f;
	[SerializeField]
	private float _dashMultiplier = 2.0f;
	private bool _dashInput = false;
	private bool _isDashing = false;
	#endregion


	// Start is called before the first frame update
	void Start() {
		_controller = GetComponent<CharacterController>();
		#region Events subscription
		// Subscribe to swiping functionalities
		SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		// SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		// SwipeController.Instance.onSwipeRight += OnSwipeRight;
		SwipeController.Instance.onTap += OnTap;
		#endregion
	}

	// Update is called once per frame
	void Update() {
		Vector3 velocity = Vector3.forward * _speed;
		if (_isDashing) {
			velocity *= _dashMultiplier;
		}

		if (_controller.isGrounded) {
			if (_jumpInput) {
				_yVelocity = _jumpHeight;
				_jumpInput = false;
				_canAirMove = true;
			}
		} else {
			_yVelocity -= _gravity;
			if (Math.Abs(_yVelocity) <= _maxDoubleJumpVelocity && _canAirMove) {
				_canAirMove = true;
			}

			if (_canAirMove) {
				if (_jumpInput) {
					_yVelocity += _jumpHeight;
					_jumpInput = false;
					_canAirMove = false;
				} else if (_dashInput) {
					StartCoroutine("Dash");
					_dashInput = false;
					_canAirMove = false;
				}
			}
		}

		velocity.y = _yVelocity;
		_controller.Move(velocity * Time.deltaTime);
	}

	private void OnTap() {
		if (_controller.isGrounded || _canAirMove) {
			_jumpInput = true;
		}
	}
	private void OnSwipeUp() {
		if (!_controller.isGrounded && _canAirMove) {
			_dashInput = true;
		}
	}
	private void OnSwipeDown() {
		if (_canCrouch && _controller.isGrounded) {
			_canCrouch = false;
			_originalControllerheight = _controller.height;
			_controller.height = _crouchResize;
			StartCoroutine("Crouch");
		}
	}
	private IEnumerator Dash() {
		_isDashing = true;
		yield return new WaitForSeconds(_dashDuration);
		_isDashing = false;
	}
	private IEnumerator Crouch() {
		yield return new WaitForSeconds(_crouchDuration);
		_controller.height = _originalControllerheight;
		_canCrouch = true;
	}
}
