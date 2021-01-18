using UnityEngine;

public class PlayerController : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		// Subscribe to swiping functionalities
		SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		SwipeController.Instance.onSwipeRight += OnSwipeRight;
	}

	// Update is called once per frame
	private void OnSwipeUp() {
		Debug.Log("Swiped Up");
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
