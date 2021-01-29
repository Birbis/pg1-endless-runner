using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ScrollPlaneController : MonoBehaviour {

	#region Variables

	private static ScrollPlaneController _instance;
	public static ScrollPlaneController Instance { get { return _instance; } }

	[SerializeField,
	Tooltip("This contains all the tiles to spawn")]
	private List<TileScriptableObject> _tiles;

	[SerializeField,
	Tooltip("The initial tiles speed"),
	Range(1.0f, 20.0f)]
	private float _initialSpeed;

	[SerializeField,
	Tooltip("Holds the initial tile, then the current tile the player is stepping on")]
	private TileScriptableObject _currentTile;

	[SerializeField,
	Tooltip("The minimum number of tiles ahead to constantly have "
	+ "\nNB: Must never be equal to the number of objects inside the list of tiles!"),
	Range(1, 10)]
	private int _minTilesAhead;

	private Queue<TileScriptableObject> _nextTiles;
	private Dictionary<string, Dictionary<int, List<TileScriptableObject>>> poolDictionary;
	private bool _playing;
	private string _currentStage;
	private float _lengthOffset = 0;
	private int _difficulty;
	#endregion

	private void Awake() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion

		if (_currentTile == null) Debug.LogWarning("[ScrollPlaneController]::Awake - Current tile not found");
		if (_tiles == null || _tiles.Count == 0) Debug.LogWarning("[ScrollPlaneController]::Awake - Tiles list is empty or null!");
		if (float.IsNaN((float)_minTilesAhead)) Debug.LogWarning("[ScrollPlaneController]::Awake - Min Tiles Ahead has not been initialized!");
		_playing = false;
	}

	private void OnEnable() {
		try {
			#region Pool instantiation
			// Pool initialization
			poolDictionary = new Dictionary<string, Dictionary<int, List<TileScriptableObject>>>();
			_nextTiles = new Queue<TileScriptableObject>();

			foreach (TileScriptableObject tileSO in _tiles) {
				if (!poolDictionary.ContainsKey(tileSO.category)) {
					poolDictionary[tileSO.category] = new Dictionary<int, List<TileScriptableObject>>();
				}

				if (!poolDictionary[tileSO.category].ContainsKey(tileSO.difficulty)) {
					poolDictionary[tileSO.category][tileSO.difficulty] = new List<TileScriptableObject>();
				}
				tileSO.sceneObj = Instantiate(tileSO.tile);
				tileSO.sceneObj.SetActive(false);
				poolDictionary[tileSO.category][tileSO.difficulty].Add(tileSO);
			}
			#endregion

			#region Current Tile first instantiation
			// * _currentTile is initialized in editor
			_currentTile.sceneObj = Instantiate(_currentTile.tile);
			_currentTile.sceneObj.SetActive(false);
			setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
			// poolDictionary[_currentTile.category].Add(_currentTile);
			setTileAttributes(_currentTile.sceneObj, _currentTile.sceneObj.transform.position, _currentTile.sceneObj.transform.rotation);
			#endregion

			#region Events subscription
			// Subscribe to onStageUpdate functionality
			GameManager.Instance.onStageUpdate += OnStageUpdate;
			GameManager.Instance.onDifficultyUpdate += OnDifficultyUpdate;
			#endregion

			ScoreManager.Instance.StartScoreCounter = true;
			_playing = true;
		} catch (Exception e) {
			Debug.LogError(e);
		}
	}

	private void Update() {
		if (_playing) {
			try {
				if (_nextTiles.Count < _minTilesAhead) {
					if ((_currentTile.length / 2 + _currentTile.sceneObj.transform.position.z < _currentTile.maxDistance)) {
						spawnFromPool(_currentStage);
					}
				} else {
					if (Mathf.Abs(_currentTile.sceneObj.transform.position.z) >= _currentTile.length / 2) {
						_currentTile.sceneObj.SetActive(false);
						_currentTile = _nextTiles.Dequeue();
						_lengthOffset -= _currentTile.length;
					} else {
						foreach (TileScriptableObject tile in _nextTiles) {
							setTileVelocity(tile.sceneObj.GetComponent<Rigidbody>());
						}
					}
				}
				setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
			} catch (System.Exception e) {
				Debug.LogError(e);
			}
		}
	}

	private void spawnFromPool(string category) {
		if (!poolDictionary.ContainsKey(category)) {
			Debug.LogWarning("Category " + category + " doesn't exist");
			return;
		}

		try {
			List<TileScriptableObject> pool = poolDictionary[category][_difficulty];
			// either the last tile in the queue or the current tile
			TileScriptableObject lastTile = _nextTiles.Count == 0 ? _currentTile : (_nextTiles.Peek());
			// object reference for the next picked tile
			TileScriptableObject pickedTile;
			do {
				// Fetches random object until it is different to the current one or it is contained in the queue
				pickedTile = pool[Random.Range(0, pool.Count - 1)];
			} while (_nextTiles.Contains(pickedTile) || GameObject.ReferenceEquals(pickedTile.sceneObj, _currentTile.sceneObj));

			// Calculates the position for the tile to "spawn"
			Transform objTransform = lastTile.sceneObj.transform;
			Vector3 position = objTransform.position;
			Quaternion rotation = objTransform.rotation;
			position.z += (_currentTile.length / 2) + _lengthOffset + (lastTile.length / 2);

			// Randomizes rotation on the y axis
			if (Random.value < 0.5f) rotation = Quaternion.Euler(rotation.eulerAngles + 180f * Vector3.up);
			setTileAttributes(pickedTile.sceneObj, position, rotation);

			// Enqueues the picked tile and adds the length, if necessary, to the needed offset
			_nextTiles.Enqueue(pickedTile);
			if (_nextTiles.Count > 1) _lengthOffset += pickedTile.length;
		} catch (System.Exception e) {
			Debug.LogError(e);
		}
	}

	private void setTileVelocity(Rigidbody rb) {
		rb.velocity = Vector3.back * _initialSpeed;
	}

	private void setTileAttributes(GameObject obj, Vector3 position, Quaternion rotation) {
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		FadeIn fadeInScript = obj.GetComponent<FadeIn>();
		if (fadeInScript != null) {
			fadeInScript.TriggerFadeIn();
		}
	}

	public void EnableScroller() {
		gameObject.SetActive(true);
	}

	private void OnStageUpdate(string newStage) {
		_currentStage = newStage;
	}

	private void OnDifficultyUpdate(int newDifficulty) {
		_difficulty = newDifficulty;
	}
}