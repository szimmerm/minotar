using UnityEngine;
using System.Collections;

public class MinotarWalkRun : MonoBehaviour {

	public float run_speed;
	public float run_time;
	
	public float walk_speed;
	public float walk_time;

	private bool is_running = false;
	private GameControllerScript game_controller;
	private MovingObject move_controller;

	private MinotarPerception player;
//	private float trigger_distance;
	public bool force_run;

	public bool player_in_zone = false;

	void Awake() {
		game_controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControllerScript>();
		move_controller = GetComponent<MovingObject>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MinotarPerception>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(distance_switcher());
	}
	
	public void walk() {
		move_controller.max_velocity = walk_speed;
	}

	public void run() {
		move_controller.max_velocity = run_speed;
	}

	/* change le comportement du minotar en fonction de la distance */
	private IEnumerator distance_switcher() {
		while(!game_controller.is_game_over) {
			if (force_run || player.detected) {
				run();
			} else {
				walk();
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	/* change le comportement du minotar de maniere reguliere */
	private IEnumerator timed_switcher() {
		while(!game_controller.is_game_over) {
			float new_speed;
			float new_timer;
			if (is_running) {
				new_speed = walk_speed;
				new_timer = walk_time;
			} else {
				new_speed = run_speed;
				new_timer = run_time;
			}
			move_controller.max_velocity = new_speed;
			is_running = !is_running;
			yield return new WaitForSeconds(new_timer);
		}
	}
}