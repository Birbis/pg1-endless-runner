using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	#region Variables
	private static ScoreManager _instance;
	public static ScoreManager Instance { get { return _instance; } }
	private bool _startScoreCounter = false;

	public bool StartScoreCounter { set { _startScoreCounter = value; } }

	private float _distance = 0;

	public float distance {
		get => _distance;
		set {
			_distance = value;
			if (onDistanceUpdate != null) onDistanceUpdate(value);
		}
	}

	private const float _scorePerSecond = 10;

	private float _score = 0;

	[SerializeField,
	Tooltip("The text component that holds the UI score")]
	private Text _IUTextField;

	[SerializeField,
	Tooltip("The score text prefix/suffix. Initialized to the empty string")]
	private string _scorePrefix, _scoreSuffix;

	public event Action<float> onDistanceUpdate;
	#endregion

	private void Awake() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion

		if (_IUTextField == null) Debug.LogWarning("[ScoreManager]::Awake - UI Text Field component not found");
	}

	private void Start() {
		#region Variables initialization
		// * init score to zero. Can it start with something else (powerups, etc...)?
		if (_scorePrefix == null) _scorePrefix = "";
		if (_scoreSuffix == null) _scoreSuffix = "";
		#endregion
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (_startScoreCounter) {
			// TODO: wouldn't be bad to not retrieve EVERY time the speed
			distance += Time.deltaTime * ScrollPlaneController.Instance.speed;
			_score = distance * _scorePerSecond;
			_IUTextField.text = _scorePrefix + " " + (int)_score + " " + _scoreSuffix;
		}
	}
}
