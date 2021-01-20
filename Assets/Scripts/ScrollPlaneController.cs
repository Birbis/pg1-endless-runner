using System;
using System.Collections.Generic;
using UnityEngine;

public class ScrollPlaneController : MonoBehaviour {
	private static ScrollPlaneController _instance;
	public static ScrollPlaneController Instance { get { return _instance; } }

	[SerializeField,
	Tooltip("This contains all the tiles to spawn")]
	private List<GameObject> tiles;

	[SerializeField,
	Tooltip("The initial tiles speed"),
	Range(1.0f, 20.0f)]
	private float _initialSpeed;

	[SerializeField,
	Tooltip("The difference between the current tile axis value and the player's position (supposed to be [0,0,0])")]
	private float _maxDifference;

	[SerializeField,
	Tooltip("Holds the initial tile, then the current tile the player is stepping on")]
	private GameObject _currentTile;
	private GameObject currentTile {
		get { return _currentTile; }
		set {
			if (value != null) {
				_currentTileRB = value.GetComponent<Rigidbody>();
			}
			_currentTile = value;
		}
	}
	private Rigidbody _currentTileRB;
	// Start is called before the first frame update
	private void Awake() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion

		if (_currentTile != null) _currentTileRB = _currentTile.GetComponent<Rigidbody>();
		else Debug.LogWarning("[ScrollPlaneController]::Awake - Current tile not found");
	}

	private void FixedUpdate() {
		// Constant tile speed
		_currentTileRB.velocity = Vector3.forward * _initialSpeed;
		if (Mathf.Abs(_currentTile.transform.position.z) >= _maxDifference) spawnNextTile();
	}


	public void spawnNextTile() {
		Debug.Log("I am spawning the next tile");
	}
}
