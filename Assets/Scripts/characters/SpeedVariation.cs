using UnityEngine;
using System.Collections;

public class SpeedVariation : MonoBehaviour {

	public float walk_speed;
	public float run_speed;
	public float dash_speed;

	public bool is_dashing = false;

	private float current_speed;

	private MovementScript move;

	// Use this for initialization
	void Awake() {
		move = GetComponent<MovementScript>();
	}

	void Start () {
		move.max_velocity = walk_speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (!is_dashing) {
			update_speed();
		}
	}
	
	private void update_speed() {
		move.max_velocity = current_speed;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			current_speed = run_speed;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			current_speed = walk_speed;
		}
	}

	public void start_charge() {
		is_dashing = true;
		move.max_velocity = dash_speed;
	}

	public void stop_charge() {
		is_dashing = false;
		move.max_velocity = current_speed;
	}
}