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

	private TileScriptableObject _nextTile;
	private Dictionary<string, List<TileScriptableObject>> poolDictionary;

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
	}

	private void Start() {
		try {
			#region Pool instantiation
			// Pool initialization
			poolDictionary = new Dictionary<string, List<TileScriptableObject>>();

			foreach (TileScriptableObject tileSO in _tiles) {
				if (!poolDictionary.ContainsKey(tileSO.category)) {
					poolDictionary[tileSO.category] = new List<TileScriptableObject>();
				}
				tileSO.sceneObj = Instantiate(tileSO.tile);
				tileSO.sceneObj.SetActive(false);
				poolDictionary[tileSO.category].Add(tileSO);
			}
			#endregion

			#region Current Tile first instantiation
			// * _currentTile is initialized in editor
			_currentTile.sceneObj = Instantiate(_currentTile.tile);
			_currentTile.sceneObj.SetActive(false);
			setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
			poolDictionary[_currentTile.category].Add(_currentTile);
			setTileAttributes(_currentTile.sceneObj, new Vector3(0, 0, 0), Quaternion.identity);
			#endregion

			ScoreManager.Instance.StartScoreCounter = true;
		} catch (Exception e) {
			Debug.LogError(e);
		}
	}

	private void Update() {
		try {
			if (_nextTile == null) {
				if ((_currentTile.length / 2 + _currentTile.sceneObj.transform.position.z < _currentTile.maxDistance)) {
					Transform objTransform = _currentTile.sceneObj.transform;
					Vector3 position = objTransform.position;
					position.z += (_currentTile.length / 2) + (_currentTile.length / 2);
					// TODO: refactor the "Test" category. Needs to be the current game phase from the GameManager
					spawnFromPool("Test", position, objTransform.rotation);
				}
			} else {
				if (Mathf.Abs(_currentTile.sceneObj.transform.position.z) >= _currentTile.length / 2) {
					_currentTile.sceneObj.SetActive(false);
					_currentTile = _nextTile;
					_nextTile = null;
				} else {
					setTileVelocity(_nextTile.sceneObj.GetComponent<Rigidbody>());
				}
			}
			setTileVelocity(_currentTile.sceneObj.GetComponent<Rigidbody>());
		} catch (System.Exception e) {
			Debug.LogError(e);
		}
	}

	private void spawnFromPool(string category, Vector3 position, Quaternion rotation) {
		if (!poolDictionary.ContainsKey(category)) {
			Debug.LogWarning("Category " + category + " doesn't exist");
			return;
		}

		try {
			List<TileScriptableObject> pool = poolDictionary[category];
			do {
				// Fetches random object until it is different to the current one
				_nextTile = pool[Random.Range(0, pool.Count - 1)];
			} while (GameObject.ReferenceEquals(_nextTile.sceneObj, _currentTile.sceneObj));
			setTileAttributes(_nextTile.sceneObj, position, rotation);
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
	}
}