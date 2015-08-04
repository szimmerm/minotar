using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementScript))]

public class MinotarController : MonoBehaviour {

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
	}
	
	private void go_towards_point(Vector2 point) {
		Vector2 minus = (point - (Vector2)transform.position);
		move.direction = minus;
	}
	
	private void update_direction() {
		go_towards_point(pathfinder.get_smooth_next_move(transform.position));
	}
}
