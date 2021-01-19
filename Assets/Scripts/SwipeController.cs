using UnityEngine;
using System;

public class SwipeController : MonoBehaviour {
	// Singleton reference
	private static SwipeController _instance;
	public static SwipeController Instance { get { return _instance; } }

	#region Private Variables
	private bool isDragging = false;
	private Vector2 startTouch, swipeDelta;
	private const int DEADZONE = 125;
	#endregion

	#region Public Getters/Setters
	public event Action onSwipeUp;
	public event Action onSwipeDown;
	public event Action onSwipeLeft;
	public event Action onSwipeRight;
	#endregion

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	private void Update() {

		#region Standalone Inputs

		if (Input.GetMouseButtonDown(0)) {
			isDragging = true;
			startTouch = Input.mousePosition;
		} else if (Input.GetMouseButtonUp(0)) {
			isDragging = false;
			Reset();
		}

		#endregion

		#region Mobile Inputs

		if (Input.touches.Length > 0) {
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Began) {
				// User started touching the screen, start position handling
				isDragging = true;
				startTouch = touch.position;
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				// Consider both Ended and Canceled phases for resetting
				isDragging = false;
				Reset();
			}
		}

		#endregion

		#region Calculate Inputs
		// Calculate distance from zero.
		swipeDelta = Vector2.zero;
		if (isDragging) {
			if (Input.touches.Length > 0) swipeDelta = Input.touches[0].position - startTouch;
			else if (Input.GetMouseButton(0)) swipeDelta = (Vector2)Input.mousePosition - startTouch;
		}

		// Check if deadzone has been crossed 
		if (swipeDelta.magnitude > DEADZONE) {
			float x = swipeDelta.x;
			float y = swipeDelta.y;

			if (Mathf.Abs(x) > Mathf.Abs(y)) {
				// Left or right
				if (x < 0) SwipeLeft();
				else SwipeRight();
			} else {
				// Up or down
				if (y < 0) SwipeDown();
				else SwipeUp();
			}
			Reset();
		}
		#endregion
	}

	private void Reset() {
		startTouch = swipeDelta = Vector2.zero;
		isDragging = false;
	}
	private void SwipeUp() {
		if (onSwipeUp != null) onSwipeUp();
	}
	private void SwipeDown() {
		if (onSwipeDown != null) onSwipeDown();
	}
	private void SwipeLeft() {
		if (onSwipeLeft != null) onSwipeLeft();
	}
	private void SwipeRight() {
		if (onSwipeRight != null) onSwipeRight();
	}
}
