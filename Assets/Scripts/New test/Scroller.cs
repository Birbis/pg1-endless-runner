using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

	[SerializeField]
	private string _stage = "Egypt";

	[SerializeField]
	private float _initialOffset = 10.0f;

	private TileScriptableObject _initialStageTile;
	private TileScriptableObject _finalStageTile;
	private TileScriptableObject[] _stageTiles;

	private float _zOffset = 0.0f;

	private void Awake() {
		RetrieveTiles(_stage);
	}

	private void RetrieveTiles(string stage) {
		_stageTiles = Shuffle<TileScriptableObject>(Resources.LoadAll<TileScriptableObject>("Tiles/" + stage));
		for (int i = 0; i < _stageTiles.Length; i++) {
			TileScriptableObject current = _stageTiles[i];
			if (current.isInitial) _initialStageTile = current;
			if (current.isFinal) _finalStageTile = current;

			float zPosition = i * current.length + _initialOffset;
			Instantiate(current.tile, new Vector3(0, 0, zPosition), Quaternion.Euler(0, Random.Range(0, 1) >= .5 ? 0 : 180, 0));
			_zOffset = zPosition;
		}
	}

	public void RecycleTile(GameObject tile, float length) {
		tile.transform.position = new Vector3(0, 0, _zOffset);
		Debug.Log("Recycling tile " + tile.name + " with z: " + _zOffset);
		_zOffset += (length + _initialOffset);
	}

	public T[] Shuffle<T>(T[] list) {
		for (int t = 0; t < list.Length; t++) {
			T tmp = list[t];
			int r = Random.Range(t, list.Length);
			list[t] = list[r];
			list[r] = tmp;
		}

		return list;
	}
}

