using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ScrollPlaneController : MonoBehaviour {

	// #region Variables
	private static ScrollPlaneController _instance;
	public static ScrollPlaneController Instance { get { return _instance; } }

	[SerializeField,
	Tooltip("The initial tiles speed"),
	Range(1.0f, 20.0f)]
	private float _initialSpeed;

	public float speed {
		get => _initialSpeed;
		set {
			_initialSpeed = value;
		}
	}

	// [SerializeField,
	// Tooltip("Holds the initial tile, then the current tile the player is stepping on")]
	// private TileScriptableObject _currentTile;

	// [SerializeField,
	// Tooltip("The minimum number of tiles ahead to constantly have "
	// + "\nNB: Must never be equal to the number of objects inside the list of tiles!"),
	// Range(1, 10)]
	// private int _minTilesAhead;

	// [SerializeField,
	// Tooltip("The minimum distance run per stage")]
	// private List<StageScriptableObject> _stages;

	// private Queue<TileScriptableObject> _nextTiles;
	// private Dictionary<string, Dictionary<int, List<TileScriptableObject>>> _poolDictionary;
	// private bool _playing;
	// private string _currentStage;
	// private float _lengthOffset = 0;
	// private int _difficulty;
	// #endregion

	// private void Awake() {
	// 	#region Singleton
	// 	if (_instance != null && _instance != this) {
	// 		Destroy(this.gameObject);
	// 	} else {
	// 		_instance = this;
	// 	}
	// 	#endregion

	// 	if (_currentTile == null) Debug.LogWarning("[ScrollPlaneController]::Awake - Current tile not found");
	// 	if (float.IsNaN((float)_minTilesAhead)) Debug.LogWarning("[ScrollPlaneController]::Awake - Min Tiles Ahead has not been initialized!");
	// 	_playing = false;
	// }

	// private void Start() {
	// 	#region Load Resources
	// 	TileScriptableObject[] resourcesTiles = Resources.LoadAll<TileScriptableObject>("Tiles");
	// 	if (resourcesTiles == null || resourcesTiles.Length == 0) Debug.LogWarning("[ScrollPlaneController]::Start - Tiles list is empty or null!");
	// 	_stages = new List<StageScriptableObject>(Resources.LoadAll<StageScriptableObject>("Stages"));
	// 	if (_stages == null || _stages.Count == 0) Debug.LogWarning("[ScrollPlaneController]::Start - Tiles list is empty or null!");
	// 	#endregion

	// 	#region Pool instantiation
	// 	// Pool initialization
	// 	_poolDictionary = new Dictionary<string, Dictionary<int, List<TileScriptableObject>>>();
	// 	_nextTiles = new Queue<TileScriptableObject>();

	// 	foreach (TileScriptableObject tileSO in resourcesTiles) {
	// 		if (!_poolDictionary.ContainsKey(tileSO.category)) {
	// 			_poolDictionary[tileSO.category] = new Dictionary<int, List<TileScriptableObject>>();
	// 		}

	// 		if (!_poolDictionary[tileSO.category].ContainsKey(tileSO.difficulty)) {
	// 			_poolDictionary[tileSO.category][tileSO.difficulty] = new List<TileScriptableObject>();
	// 		}
	// 		tileSO.sceneObj = Instantiate(tileSO.tile);
	// 		tileSO.sceneObj.SetActive(false);
	// 		_poolDictionary[tileSO.category][tileSO.difficulty].Add(tileSO);
	// 	}
	// 	#endregion

	// 	#region Current Tile first instantiation
	// 	// * _currentTile is initialized in editor
	// 	_currentTile.sceneObj = Instantiate(_currentTile.tile);
	// 	_currentTile.sceneObj.SetActive(false);
	// 	setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
	// 	setTileAttributes(_currentTile.sceneObj, _currentTile.sceneObj.transform.position, _currentTile.sceneObj.transform.rotation);
	// 	#endregion
	// }

	// private void OnEnable() {
	// 	try {
	// 		#region Events subscription
	// 		// Subscribe to onStageUpdate functionality
	// 		GameManager.Instance.onStageUpdate += OnStageUpdate;
	// 		GameManager.Instance.onDifficultyUpdate += OnDifficultyUpdate;
	// 		ScoreManager.Instance.onDistanceUpdate += OnDistanceUpdate;
	// 		#endregion

	// 		ScoreManager.Instance.StartScoreCounter = true;
	// 		_playing = true;
	// 	} catch (Exception e) {
	// 		Debug.LogError(e);
	// 	}
	// }

	// private void Update() {
	// 	if (_playing) {
	// 		try {
	// 			if (_nextTiles.Count < _minTilesAhead) {
	// 				if ((_currentTile.length / 2 + _currentTile.sceneObj.transform.position.z < _currentTile.maxDistance)) {
	// 					spawnFromPool(_currentStage);
	// 				}
	// 			} else {
	// 				if (Mathf.Abs(_currentTile.sceneObj.transform.position.z) >= _currentTile.length / 2) {
	// 					_currentTile.sceneObj.SetActive(false);
	// 					_currentTile = _nextTiles.Dequeue();
	// 					_lengthOffset -= _currentTile.length;
	// 				} else {
	// 					foreach (TileScriptableObject tile in _nextTiles) {
	// 						setTileVelocity(tile.sceneObj.GetComponent<Rigidbody>());
	// 					}
	// 				}
	// 			}
	// 			setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
	// 		} catch (System.Exception e) {
	// 			Debug.LogError(e);
	// 		}
	// 	}
	// }

	// private void spawnFromPool(string category) {
	// 	if (!_poolDictionary.ContainsKey(category)) {
	// 		Debug.LogWarning("Category " + category + " doesn't exist");
	// 		return;
	// 	}

	// 	try {
	// 		List<TileScriptableObject> pool = _poolDictionary[category][_difficulty];
	// 		// either the last tile in the queue or the current tile
	// 		TileScriptableObject lastTile = _nextTiles.Count == 0 ? _currentTile : (_nextTiles.Peek());
	// 		// object reference for the next picked tile
	// 		TileScriptableObject pickedTile;
	// 		do {
	// 			// Fetches random object until it is different to the current one or it is contained in the queue
	// 			pickedTile = pool[Random.Range(0, pool.Count - 1)];
	// 		} while (_nextTiles.Contains(pickedTile) || GameObject.ReferenceEquals(pickedTile.sceneObj, _currentTile.sceneObj));

	// 		// Calculates the position for the tile to "spawn"
	// 		Transform objTransform = lastTile.sceneObj.transform;
	// 		Vector3 position = objTransform.position;
	// 		Quaternion rotation = objTransform.rotation;
	// 		position.z += (_currentTile.length / 2) + _lengthOffset + (lastTile.length / 2);

	// 		// Randomizes rotation on the y axis
	// 		if (Random.value < 0.5f) rotation = Quaternion.Euler(rotation.eulerAngles + 180f * Vector3.up);
	// 		setTileAttributes(pickedTile.sceneObj, position, rotation);

	// 		// Enqueues the picked tile and adds the length, if necessary, to the needed offset
	// 		_nextTiles.Enqueue(pickedTile);
	// 		if (_nextTiles.Count > 1) _lengthOffset += pickedTile.length;
	// 	} catch (System.Exception e) {
	// 		Debug.LogError(e);
	// 	}
	// }

	// private void setTileVelocity(Rigidbody rb) {
	// 	rb.velocity = Vector3.back * _initialSpeed;
	// }

	// private void setTileAttributes(GameObject obj, Vector3 position, Quaternion rotation) {
	// 	obj.SetActive(true);
	// 	obj.transform.position = position;
	// 	obj.transform.rotation = rotation;

	// 	FadeIn fadeInScript = obj.GetComponent<FadeIn>();
	// 	if (fadeInScript != null) {
	// 		fadeInScript.TriggerFadeIn();
	// 	}
	// }

	public void EnableScroller() {
		// 	gameObject.SetActive(true);
	}

	// private void OnStageUpdate(string newStage) {
	// 	_currentStage = newStage;
	// }

	// private void OnDifficultyUpdate(int newDifficulty) {
	// 	_difficulty = newDifficulty;
	// }

	// private void OnDistanceUpdate(float newDistance) {
	// 	StageScriptableObject newStage = null;
	// 	foreach (StageScriptableObject stage in _stages) {
	// 		if (stage.stageName == _currentStage) {
	// 			newStage = stage;
	// 			break;
	// 		}
	// 	}
	// 	if (newStage == null) {
	// 		Debug.LogError("[ScrollPlaneController]::OnDistanceUpdate - Stage Lengths does not contain stage " + _currentStage);
	// 		return;
	// 	} else if (newDistance >= newStage.length) {
	// 		_currentStage = newStage.stageName;
	// 		GameManager.Instance.stage = _currentStage;
	// 	}
	// }
}