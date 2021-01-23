using UnityEngine;

public class UIManager : MonoBehaviour {

	[SerializeField,
	Tooltip("The scroll controller script in the scene")]
	private ScrollPlaneController _scrollController;

	[SerializeField,
	Tooltip("The player controller script in the scene")]
	private PlayerController _playerController;

	[SerializeField,
	Tooltip("The game object containing the whole menu UI")]
	private GameObject _menuUI;
	public void OnMenuPlayButtonPressed() {
		_menuUI.SetActive(false);
		_scrollController.EnableScroller();
		_playerController.EnablePlayer();
	}
}
