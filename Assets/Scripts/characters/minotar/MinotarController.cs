using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementScript))]

public class MinotarController : MonoBehaviour {

	public float walk_speed;
	public float run_speed;

	public bool sees_player = false;
	public bool should_update_speed = true;

	private MovementScript move;
	private PathfindingManager pathfinder;
	private bool should_update_direction = true;

	void Awake() {
		move = GetComponent<MovementScript>();
		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();
	}

	// Update is called once per frame
	void Update () {
		if(should_update_direction) {
			update_direction();
		}
		if (should_update_speed) {
			update_speed();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			sees_player = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			sees_player = false;
		}
	}
	
	private void update_speed() {
		if (sees_player) {
			move.max_velocity = run_speed;
		} else {
			move.max_velocity = walk_speed;
		}
	}

	private void update_direction() {
		go_towards_point(pathfinder.get_smooth_next_move(transform.position));
	}

	private void go_towards_point(Vector2 point) {
		Vector2 minus = (point - (Vector2)transform.position);
		move.direction = minus;
	}
}
