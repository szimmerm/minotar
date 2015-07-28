using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour, IReset {

	public bool is_game_over = false;
	private HighScoreScript high_score;
	private HashSet<IReset> active_objects;
	private CreatureSpawner creature_spawner;
	private ObstacleSpawner obstacle_spawner;

	void Awake() {
		high_score = GetComponent<HighScoreScript>();
		active_objects = new HashSet<IReset>();
		creature_spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<CreatureSpawner>();
		obstacle_spawner = GameObject.FindGameObjectWithTag ("ObstacleSpawner").GetComponent<ObstacleSpawner>();
	}

	// Use this for initialization
	void Start () {
		ResetScript.register_in_controller (this);
	}

	private void restart() {
		foreach (IReset component in active_objects) {
			component.on_reset();
		}
		high_score.restart ();
		obstacle_spawner.restart();
		
//		Application.LoadLevel (Application.loadedLevel);
	}
	
	// Update is called once per frame
	void Update () {
		if(is_game_over && Input.GetKeyDown("r")) {
			restart();
		}
	}

	public void register_object(IReset comp) {
		active_objects.Add (comp);
	}

	public void unregister_object(IReset comp) {
		active_objects.Remove(comp);
	}

	public void game_over() {
		high_score.pause_score ();
		is_game_over = true;
		creature_spawner.stop_spawning();
		creature_spawner.send_game_over_message();
		obstacle_spawner.stop_spawning();
	}

	public void on_reset() {
		is_game_over = false;
	}
}
