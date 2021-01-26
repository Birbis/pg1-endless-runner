using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {
	[NonSerializedAttribute] List<Material> _materials;
	// Start is called before the first frame update
	void Awake() {
		_materials = new List<Material>();
		foreach (Renderer rend in GetComponentsInChildren<Renderer>(true)) {
			foreach (Material mat in rend.materials) {
				if (mat.HasProperty("_TriggerTime")) {
					_materials.Add(mat);
				}
			}
		}
	}

	public void TriggerFadeIn() {
		if (_materials != null) {
			foreach (Material mat in _materials) {
				Debug.Log(Time.timeSinceLevelLoad);
				mat.SetFloat("_TriggerTime", Time.timeSinceLevelLoad);
			}
		}
	}

}
