using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour, IReset {

	private Vector3 start_position;

	void Awake() {
		start_position = transform.position;
	}

	void Start() {
		ResetScript.register_in_controller(this);
	}

	public void on_reset() {
		transform.position = start_position;
	}

	public static void register_in_controller(IReset reset_script) {
		GameControllerScript main_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		main_controller.register_object (reset_script);
	}
}