using Moves;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour, Move {

	private Dictionary<string, GameObject> _lanes;
	private string _currentLane;
	private bool _freezed;
	private Transform _dodgingTransform;
	[SerializeField,
	Tooltip("The lerping speed for the dodge (should be proportional to the animation of the dodge, if any)\nDefaults to 1.5"),
	Range(1.0f, 20.0f)]
	private float _dodgingSpeed;

	// Start is called before the first frame update
	void Start() {
		_lanes = new Dictionary<string, GameObject>();
		GameObject[] goLanes = GameObject.FindGameObjectsWithTag("Lane");
		foreach (GameObject lane in goLanes) {
			_lanes[lane.name] = lane;
		}

		_currentLane = "Center";
		if (float.IsNaN(_dodgingSpeed)) _dodgingSpeed = 1.5f;

		if (_lanes.Keys.Count != 3) Debug.LogWarning("[Dodge]::Start - Found a different amount of lanes");
	}

	private void Update() {
		if (_dodgingTransform != null) {
			if (Vector3.SqrMagnitude(transform.position - _dodgingTransform.position) > 0.0001) {
				Vector3 pos = transform.position;
				transform.position = Vector3.Lerp(pos, new Vector3(_dodgingTransform.position.x, pos.y, pos.z), Time.deltaTime * _dodgingSpeed);
			} else {
				_dodgingTransform = null;
			}
		}
	}

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
			// gameObject.transform.position = new Vector3(_dodgingTransform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		}
	}

	public bool Freezed { get => _freezed; set => _freezed = value; }
}
