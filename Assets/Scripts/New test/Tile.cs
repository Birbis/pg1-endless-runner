using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	private Scroller _scroller;

	[SerializeField]
	private float _length;

	[SerializeField]
	private float _invisibleTime;
	private void OnEnable() {
		_scroller = GameObject.FindObjectOfType<Scroller>();
	}
	private void OnBecameInvisible() {
		if (Time.time - _invisibleTime > .5f) {
			_invisibleTime = Time.time;
			_scroller.RecycleTile(this.gameObject, _length);
		}
	}
}
