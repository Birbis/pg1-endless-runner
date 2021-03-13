using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileScriptableObject", order = 1)]
public class TileScriptableObject : ScriptableObject {

	[SerializeField,
	Tooltip("The length of the tile")]
	public float length;

	[SerializeField,
	Tooltip("The max distance from the player and the end of the tile before spawning the next tile"),
	Range(0.0f, 100.0f)]
	public float maxDistance;

	[SerializeField,
	Tooltip("The prefab of the tile that needs to be spawned")]
	public GameObject tile;

	[SerializeField,
	Tooltip("The difficulty definition of the tile")]
	public int difficulty;

	[SerializeField,
	Tooltip("The difficulty definition of the tile")]
	public bool isInitial;

	[SerializeField,
	Tooltip("The difficulty definition of the tile")]
	public bool isFinal;

	[NonSerializedAttribute]
	// Needed to store the scene object reference
	public GameObject sceneObj;
}
