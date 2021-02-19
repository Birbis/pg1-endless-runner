using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	private static PlayerController _instance;
	public static PlayerController Instance { get { return _instance; } }

	[SerializeField,
	Tooltip("The jump component, should be a sibling of this script")]
	private Jump _jump;
	[SerializeField,
	Tooltip("The dodge component, should be a sibling of this script")]
	private Dodge _dodge;

	[SerializeField,
	Tooltip("Needed for ground check")]
	private GroundCheck _groundCheck;

	private void Start() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion
	}

	void OnEnable() {
		#region Events subscription
		// Subscribe to swiping functionalities
		SwipeController.Instance.onSwipeUp += OnSwipeUp;
		SwipeController.Instance.onSwipeDown += OnSwipeDown;
		SwipeController.Instance.onSwipeLeft += OnSwipeLeft;
		SwipeController.Instance.onSwipeRight += OnSwipeRight;
		SwipeController.Instance.onTap += OnTap;
		#endregion

		if (_groundCheck == null) Debug.LogWarning("[PlayerController]::Start - GroundCheck not found");
	}

	private void OnSwipeUp() {
		Debug.Log("Swiped Up");
		throw new NotImplementedException();
	}
	private void OnSwipeDown() {
		Debug.Log("Swiped Down");
		throw new NotImplementedException();
	}
	private void OnSwipeLeft() {
		_dodge.execute(goLeft: true);
	}
	private void OnSwipeRight() {
		_dodge.execute(goLeft: false);
	}

	public void EnablePlayer() {
		gameObject.SetActive(true);
	}

	public void OnTap() {
		_jump.execute(_groundCheck.isGrounded());
	}

	public void Die(bool dieDefinitevely) {
		if (dieDefinitevely) {
			// Reload the scene, might need refactoring once the menus are done and ready
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		} else {
			// destroy the shield perk that it has
		}
	}

	public void Scare(bool goLeft) {
		_dodge.execute(goLeft: goLeft);
	}
}
