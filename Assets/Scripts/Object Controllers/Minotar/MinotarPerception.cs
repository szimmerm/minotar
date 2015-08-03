using UnityEngine;
using System.Collections;

public class MinotarPerception : MonoBehaviour, IReset {

	public bool detected = false;

	// Use this for initialization

	void Start() {
		ResetScript.register_in_controller (this);
	}

	public void OnTriggerEnter2D() {
		detected = true;
	}

	public void OnTriggerStay2D() {
	}

	public void OnTriggerExit2D() {
		detected = false;
	}

	public void on_reset() {
		detected = false;
	}
}
