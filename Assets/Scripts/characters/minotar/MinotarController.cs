using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementScript))]

public class MinotarController : MonoBehaviour {

	public enum States {walk, stop, charge, taunted};

	public float walk_speed = 50;
	public float run_speed = 80;
	public float charge_speed = 200;

	public bool sees_player = false;
	public bool should_update_speed = true;

	private MovementScript move;
	private MovementTools tools;
	private PathfindingManager pathfinder;
	private SpeedVariation speed_manager;

	private Transform anger_indicator;
	public States state;
	private float current_speed;

	void Awake() {
		move = GetComponent<MovementScript>();
		tools = GetComponent<MovementTools>();
		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();
		foreach(Transform child in GetComponentsInChildren<Transform>()) {
			if (child.name == "colere") {
				anger_indicator = child;
			}
		}
		if (anger_indicator == null) {
			Debug.LogError ("fumee de rage non trouvee");
		} else {
			anger_indicator.gameObject.SetActive (false);
		}
		state = States.walk;
		speed_manager = GetComponent<SpeedVariation>();
	}

	void Start() {
//		move.max_velocity = walk_speed;
	}

	// Update is called once per frame
	void Update () {
		switch(state) {
			case States.walk:
				update_direction();
//				update_speed();
				break;

			case States.charge:
				break;

			case States.stop:
				move.stop ();
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			sees_player = true;
			anger_indicator.gameObject.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "DetectionZone") {
			sees_player = false;
			anger_indicator.gameObject.SetActive (false);
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
		StartCoroutine(taunt_reaction());
	}

	public IEnumerator taunt_reaction() {
		Debug.Log ("reacting to taunt");
		Transform player = GameObject.FindGameObjectWithTag("Player").transform;

//		while(Physics2D.Linecast (transform.position, player.position, 1 << LayerMask.NameToLayer ("Walls")).collider == null){
		while (!pathfinder.has_line_to_player(transform.position)) {
			yield return new WaitForFixedUpdate();
		}

		go_towards_point(player.transform.position);
//		should_update_direction = false;
//		should_update_speed = false;
		foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				box.enabled = true;
			}
		}
		speed_manager.start_charge();
		yield return StartCoroutine(minotar_charge());
		yield return new WaitForSeconds(2f);
		on_taunt_end();
	}

	private IEnumerator minotar_charge() {
		Vector2 target = move.direction;
		move.stop ();
		state = States.charge;
		yield return new WaitForSeconds(1f);
		move.direction = target;
	}

	public void on_taunt_end() {
		foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				box.enabled = false;
			}
		}
		state = States.walk;
//		should_update_direction = true;
//		should_update_speed = true;
		speed_manager.stop_charge();
	}

	public void on_game_over() {
		move.stop();
		state = States.stop;
//		move.should_update_speed = false;
//		should_update_speed = false;
		GetComponentInChildren<Animator>().SetTrigger ("playerDead");
		StopCoroutine(taunt_reaction());
	}
}
