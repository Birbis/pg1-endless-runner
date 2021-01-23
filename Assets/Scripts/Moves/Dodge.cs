using Moves;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour, Move {

	private Dictionary<string, GameObject> _lanes;
	private string _currentLane;
	private bool _freezed;
	private Transform _dodgingTransform;
	private float _dodgingSpeed = 10.0f;

	// Start is called before the first frame update
	void Start() {
		_lanes = new Dictionary<string, GameObject>();
		GameObject[] goLanes = GameObject.FindGameObjectsWithTag("Lane");
		foreach (GameObject lane in goLanes) {
			_lanes[lane.name] = lane;
		}

		_currentLane = "Center";

		if (_lanes.Keys.Count != 3) Debug.LogWarning("[Dodge]::Start - Found a different amount of lanes");
	}

	// private void Update() {
	// 	if (_dodgingTransform != null) {
	// 		if (Vector3.SqrMagnitude(transform.position - _dodgingTransform.position) <= 0.0001) {
	// 			transform.position = Vector3.Lerp(transform.position, _dodgingTransform.position, Time.deltaTime * _dodgingSpeed);
	// 		} else {
	// 			_dodgingTransform = null;
	// 		}
	// 	}
	// }

	public void execute(bool goLeft) {
		string temp = null;
		switch (_currentLane) {
			case "Left": {
					temp = goLeft ? _currentLane : "Center";
					break;
				}
			case "Center": {
					temp = goLeft ? "Left" : "Right";
					break;
				}
			case "Right": {
					temp = goLeft ? "Center" : _currentLane;
					break;
				}
			default: {
					Debug.LogWarning("[Dodge]::execute - Error in switching current lane!");
					break;
				}
		}
		if (temp != null && temp != _currentLane) {
			_currentLane = temp;
			_dodgingTransform = _lanes[_currentLane].transform;
			// TODO: needs lerping, do not keep this as it is now.
			gameObject.transform.position = new Vector3(_dodgingTransform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		}
	}

	public bool Freezed { get => _freezed; set => _freezed = value; }
}
