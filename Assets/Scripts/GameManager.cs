using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance = null;

	public event Action<string> onStageUpdate;
	public event Action<int> onDifficultyUpdate;
	[SerializeField,
	Tooltip("The current stage in which the player is playing. Initialize it as the first possible stage."
	+ "\nUsed for tile fetching in the Scroll Controller.")]
	private string _stage;

	[SerializeField,
	Tooltip("The current difficulty to which the player is playing. Initialize it as the first possible difficulty."
	+ "\nUsed for tile fetching in the Scroll Controller.")]
	private int _difficulty;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start() {
		if (_stage == null) Debug.Log("[GameManager]::Start - Stage hasn't been initialized");
		if (float.IsNaN((float)_difficulty)) Debug.Log("[GameManager]::Start - Difficulty hasn't been initialized");
	}

	// Update is called once per frame
	void Update() {

	}

	public void StartGame() {
		UpdateStage(_stage);
		OnDifficultyUpdate(_difficulty);
	}

	private void UpdateStage(string newStage) {
		if (onStageUpdate != null) onStageUpdate(newStage);
	}

	private void OnDifficultyUpdate(int newDifficulty) {
		if (onDifficultyUpdate != null) onDifficultyUpdate(newDifficulty);
	}
}
