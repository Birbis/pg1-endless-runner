using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNameSpace {

	enum Lane {
		Left, Center, Right
	}


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

		#region Dodge variables 
		[SerializeField]
		private Transform _leftLane;

		[SerializeField]
		private Transform _centerLane;

		[SerializeField]
		private Transform _rightLane;
		[SerializeField]
		private float _dodgeSpeed = 15.0f;

		private bool _isDodging = false;

		private Vector3 _dodgeEnd;
		private float _xVelocity = 0.0f;
		private Lane _currentLane = Lane.Center;
		#endregion


		// Start is called before the first frame update
		void Start() {
			_controller = GetComponent<CharacterController>();
			#region Events subscription
			// Subscribe to swiping functionalities
			SwipeController.Instance.onSwipeUp += OnSwipeUp;
			SwipeController.Instance.onSwipeDown += OnSwipeDown;
			SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
			SwipeController.Instance.onSwipeRight += OnSwipeRight;
			SwipeController.Instance.onTap += OnTap;
			#endregion

			if (_leftLane == null) Debug.LogError("Player does not have the left lane reference!");
			if (_centerLane == null) Debug.LogError("Player does not have the center lane reference!");
			if (_rightLane == null) Debug.LogError("Player does not have the right lane reference!");
		}

		// Update is called once per frame
		void Update() {
			Vector3 velocity = Vector3.forward * _speed;
			if (_isDashing) {
				velocity *= _dashMultiplier;
			}

			if (_isDodging) {
				float x = gameObject.transform.position.x;
				_xVelocity = (_dodgeEnd.x - x) * _dodgeSpeed;
				if (Math.Abs(x - _dodgeEnd.x) <= .05) {
					_isDodging = false;
					_xVelocity = 0;
				}
			}

			if (_controller.isGrounded) {
				if (_jumpInput) {
					_yVelocity = _jumpHeight;
					_jumpInput = false;
					_canAirMove = true;
				}
			} else {
				if (!_isDashing) _yVelocity -= _gravity;
				else _yVelocity = 0;
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

			velocity.x = _xVelocity;
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
		private void OnSwipeRight() {
			Swipe(true);
		}

		private void OnSwipeLeft() {
			Swipe(false);
		}

		private void OnSwipeDown() {
			if (_canCrouch && _controller.isGrounded) {
				_canCrouch = false;
				_originalControllerheight = _controller.height;
				_controller.height = _crouchResize;
				StartCoroutine("Crouch");
			}
		}

		private void Swipe(bool goRight) {
			var target = _chooseTargetLane(goRight);
			if (target != null) {
				_dodgeEnd = target.position;
				_isDodging = true;
			}
		}

		private Transform _chooseTargetLane(bool goRight) {
			switch (_currentLane) {
				case Lane.Center: {
						_currentLane = goRight ? Lane.Right : Lane.Left;
						return goRight ? _rightLane : _leftLane;
					}
				case Lane.Right: {
						if (goRight) return null;
						_currentLane = goRight ? Lane.Right : Lane.Center;
						return goRight ? _rightLane : _centerLane;
					}
				case Lane.Left: {
						if (!goRight) return null;
						_currentLane = goRight ? Lane.Center : Lane.Left;
						return goRight ? _centerLane : _leftLane;
					}
				default: return null;
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
}
