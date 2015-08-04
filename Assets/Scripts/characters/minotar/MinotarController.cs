using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementScript))]

public class MinotarController : MonoBehaviour {

	public float walk_speed;
	public float run_speed;

	public bool sees_player = false;
	public bool should_update_speed = true;

	private MovementScript move;
	private MovementTools tools;
	private PathfindingManager pathfinder;
	private bool should_update_direction = true;

	void Awake() {
		move = GetComponent<MovementScript>();
		tools = GetComponent<MovementTools>();
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

	public void on_taunt_start() {
		go_towards_point(GameObject.FindGameObjectWithTag("Player").transform.position);
		should_update_direction = false;
		should_update_speed = false;
		foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				box.enabled = true;
			}
		}
		StartCoroutine(minotar_charge());
	}

	private IEnumerator minotar_charge() {
		move.stop ();
		move.should_update_speed = false;		
		yield return new WaitForSeconds(0.3f);
		move.should_update_speed = true;
		move.max_velocity = tools.dash_factor * walk_speed;
	}

	public void on_taunt_end() {
		foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				box.enabled = false;
			}
		}
		should_update_direction = true;
		should_update_speed = true;
	}

	
	public void on_game_over() {
		move.stop();
		move.should_update_speed = false;
		should_update_speed = false;
		GetComponentInChildren<Animator>().SetTrigger ("playerDead");
	}
}
