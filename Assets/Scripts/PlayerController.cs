using UnityEngine;
public class PlayerController : MonoBehaviour {

	[SerializeField,
	Tooltip("The jump component, should be a sibling of this script")]
	private Jump jump;

	[SerializeField,
	Tooltip("Needed for ground check")]
	private GroundCheck groundCheck;

	void Start() {
		#region Events subscription
		// Subscribe to swiping functionalities
		SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		SwipeController.Instance.onSwipeRight += OnSwipeRight;
		#endregion

		if (groundCheck == null) Debug.LogWarning("[PlayerController]::Start - GroundCheck not found");
	}

	private void OnSwipeUp() {
		if (groundCheck.isGrounded()) {
			jump.execute();
		}
	}
	private void OnSwipeDown() {
		Debug.Log("Swiped Down");
	}
	private void OnSwipeLeft() {
		Debug.Log("Swiped Left");
	}
	private void OnSwipeRight() {
		Debug.Log("Swiped Right");
	}
}
