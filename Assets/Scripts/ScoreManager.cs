using UnityEngine;

public class ScoreManager : MonoBehaviour {
	#region Variables
	private static ScoreManager _instance;
	public static ScoreManager Instance { get { return _instance; } }
	private bool _startScoreCounter = false;

	public bool StartScoreCounter { set { _startScoreCounter = value; } }

	private float _score;

	private const float _scorePerSecond = 10;
	#endregion

	private void Start() {
		#region Singleton
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		#endregion
		// * init score to zero. Can it start with something else (powerups, etc...)?
		_score = 0;
	}

	// Update is called once per frame
	void Update() {
		if (_startScoreCounter) {
			_score += Time.deltaTime * _scorePerSecond;
			Debug.Log((int)_score);
		}
	}
}
