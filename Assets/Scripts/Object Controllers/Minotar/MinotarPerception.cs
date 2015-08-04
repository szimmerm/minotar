using UnityEngine;
using System.Collections;

public class MinotarPerception : MonoBehaviour{

	public bool detected = false;

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
