using UnityEngine;
using System.Collections;

public class MinotarWalkRun : MonoBehaviour {

	public float run_speed;
	public float run_time;
	
	public float walk_speed;
	public float walk_time;

	private bool run = false;
	private GameControllerScript game_controller;
	private MovingObject move_controller;

	private Transform player;
	private float trigger_distance;
	public bool force_run;

	void Awake() {
		game_controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControllerScript>();
		move_controller = GetComponent<MovingObject>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		trigger_distance = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HighScoreScript>().score_distance;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(distance_switcher());
	}
	
	/* change le comportement du minotar en fonction de la distance */
	private IEnumerator distance_switcher() {
		while(!game_controller.is_game_over) {
			if (force_run || ((Vector2)player.position - (Vector2)transform.position).magnitude < trigger_distance) {
				move_controller.max_velocity = run_speed;
			} else {
				move_controller.max_velocity = walk_speed;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	/* change le comportement du minotar de maniere reguliere */
	private IEnumerator timed_switcher() {
		while(!game_controller.is_game_over) {
			float new_speed;
			float new_timer;
			if (run) {
				new_speed = walk_speed;
				new_timer = walk_time;
			} else {
				new_speed = run_speed;
				new_timer = run_time;
			}
			move_controller.max_velocity = new_speed;
			run = !run;
			yield return new WaitForSeconds(new_timer);
		}
	}
}