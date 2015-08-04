using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour{

	private ResetManager main_controller;

	void Awake() {
		main_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<ResetManager>();
	}

	void Start() {
		main_controller.register_object (this.gameObject);
	}
	
	void OnDestroy() {
		main_controller.unregister_object (this.gameObject);
	}
}