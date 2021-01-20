using System.Security.Principal;
using System.Collections.Generic;
using UnityEngine;

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

	private GameObject _currentTileGO;

	private Dictionary<string, List<GameObject>> poolDictionary;

	#endregion
	// Start is called before the first frame update
	private void Awake() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion

		if (_currentTile == null) Debug.LogWarning("[ScrollPlaneController]::Awake - Current tile not found");
		else _currentTileGO = _currentTile.tile;
		if (_tiles == null || _tiles.Count == 0) Debug.LogWarning("[ScrollPlaneController]::Awake - Tiles list is empty or null!");
	}

	private void Start() {
		// Pool initialization
		poolDictionary = new Dictionary<string, List<GameObject>>();

		foreach (TileScriptableObject tileSO in _tiles) {
			if (!poolDictionary.ContainsKey(tileSO.category)) {
				poolDictionary[tileSO.category] = new List<GameObject>();
			}
			GameObject obj = Instantiate(tileSO.tile);
			obj.SetActive(false);
			poolDictionary[tileSO.category].Add(obj);
		}
		_currentTileGO = spawnFromPool("Test", new Vector3(0, 0, 0), Quaternion.identity);
	}

	private void FixedUpdate() {
		if (_currentTile.length / 2 + _currentTileGO.transform.position.z <= _currentTile.maxDistance) {
			Vector3 position = _currentTileGO.transform.position;
			position.z += (_currentTile.length / 2) + (_currentTile.length / 2);
			spawnFromPool("Test", position, _currentTileGO.transform.rotation);
		}
	}

	private GameObject spawnFromPool(string category, Vector3 position, Quaternion rotation) {

		if (!poolDictionary.ContainsKey(category)) {
			Debug.LogWarning("Category " + category + " doesn't exist");
			return null;
		}

		// TODO: fix randomization with already fetched object
		// Dequeues and sets active the object from the queue
		List<GameObject> pool = poolDictionary[category];
		GameObject objectToSpawn = pool[Random.Range(0, pool.Count)];
		objectToSpawn.SetActive(true);
		// _currentTileGO.SetActive(false); // * Is this good enough? Is this erased when it should be? Should it be later on?

		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		objectToSpawn.GetComponent<Rigidbody>().velocity = Vector3.back * _initialSpeed;

		// poolDictionary[category].Enqueue(_currentTileGO);
		return objectToSpawn;
	}
}
