using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/StageScriptableObject", order = 2)]
public class StageScriptableObject : ScriptableObject {

	[SerializeField,
	Tooltip("The name of the stage")]
	public string stageName;

	[SerializeField,
	Tooltip("The length of the stage"),
	Range(1.0f, 300.0f)]
	public float length;
}
