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

	void Awake() {
		game_controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControllerScript>();
		move_controller = GetComponent<MovingObject>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(walk_run_switcher());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator walk_run_switcher() {
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
			Debug.Log ("Changement de vitesse !");
			run = !run;
			yield return new WaitForSeconds(new_timer);
			
		}
	}
}