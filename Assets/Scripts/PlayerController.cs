using UnityEngine;
public class PlayerController : MonoBehaviour {

	[SerializeField,
	Tooltip("The jump component, should be a sibling of this script")]
	private Jump _jump;
	[SerializeField,
	Tooltip("The dodge component, should be a sibling of this script")]
	private Dodge _dodge;

	[SerializeField,
	Tooltip("Needed for ground check")]
	private GroundCheck _groundCheck;

	void Start() {
		#region Events subscription
		// Subscribe to swiping functionalities
		SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		SwipeController.Instance.onSwipeRight += OnSwipeRight;
		#endregion

		if (_groundCheck == null) Debug.LogWarning("[PlayerController]::Start - GroundCheck not found");
	}
	
	private void OnSwipeUp() {
		if (_groundCheck.isGrounded()) {
			_jump.execute();
		}
	}
	private void OnSwipeDown() {
		Debug.Log("Swiped Down");
	}
	private void OnSwipeLeft() {
		_dodge.execute(goLeft: true);
	}
	private void OnSwipeRight() {
		_dodge.execute(goLeft: false);
	}
}
