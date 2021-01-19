using Moves;
using UnityEngine;

public class Jump : MonoBehaviour, Move {
	private bool _freezed;

	[SerializeField,
	Tooltip("The maximum force given to the jump of the player"),
	Range(1.0f, 10.0f)]
	private float _jumpForce;

	private Rigidbody _rb;

	private void Start() {
		_rb = GetComponent<Rigidbody>();
		if (_rb == null) Debug.LogWarning("[Jump]::Start - RigidBody not found");
	}

	public void execute() {
		_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
	}

	public bool Freezed { get => _freezed; set => _freezed = value; }

}