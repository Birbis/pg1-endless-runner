using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	[SerializeField,
	Tooltip("Needed to know if on collision the player must die regardless of its # of lives.")]
	private bool _isDefinitiveDeath;

	[SerializeField,
	Tooltip("Check this if it should just scare the player instead of erasing its life")]
	private bool _shouldJustScare;

	[SerializeField,
	Tooltip("If the player has been scared, then go left?\n\nNB: this is counted iff the Should Just Scare value is true")]
	private bool _ifScaredGoLeft;

	private PlayerController _pc;
	// Start is called before the first frame update
	void Start() {
		_pc = FindObjectOfType<PlayerController>();

		if (_pc == null) Debug.LogWarning("[ScareObstacle]::");
	}

	private void OnCollisionEnter(Collision other) {
		if (!_shouldJustScare) {
			if (other.gameObject.CompareTag("Player")) {
				_pc.Die(_isDefinitiveDeath);
			}
		}
	}

	public void ScarePlayer() {
		if (_shouldJustScare) {
			_pc.Scare(_ifScaredGoLeft);
		}
	}
}
