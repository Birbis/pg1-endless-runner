using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileScriptableObject", order = 1)]
public class TileScriptableObject : ScriptableObject {

	[SerializeField,
	Tooltip("The category to which the tile belongs to")]
	public string category;

	[SerializeField,
	Tooltip("The width of the tile, over which to distribute the 3 lanes"),
	Range(1.0f, 300.0f)]
	public float width;

	[SerializeField,
	Tooltip("The length of the tile"),
	Range(1.0f, 300.0f)]
	public float length;

	[SerializeField,
	Tooltip("The max distance from the player and the end of the tile before spawning the next tile"),
	Range(0.0f, 100.0f)]
	public float maxDistance;

	[SerializeField,
	Tooltip("The prefab of the tile that needs to be spawned")]
	public GameObject tile;

	[NonSerializedAttribute]
	// Needed to store the scene object reference
	public GameObject sceneObj;
}
