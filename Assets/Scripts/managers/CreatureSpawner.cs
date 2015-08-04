using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureSpawner : MonoBehaviour {

	public GameObject[] controlled_creatures;
	public Transform[] spawn_positions;
	public float spawn_interval = 20.0f;
	public GameObject spawn_animation;
	public float spawn_animation_duration;

	private HashSet<SpawnControlledElement> active_creatures;
//	private HighScoreScript highscore;

	private void Awake() {
		active_creatures = new HashSet<SpawnControlledElement>();
//		highscore = GameObject.FindGameObjectWithTag("GameController").GetComponent<HighScoreScript>();
	}

	void Start() {
		start_spawning();
	}

	private Vector3 choose_spawn_position() {
		Vector3 spawn_position = spawn_positions[Mathf.FloorToInt (Random.Range(0, spawn_positions.Length))].position;
		return new Vector3(spawn_position.x, spawn_position.y, 2);
	}

	private GameObject choose_creature_type() {
		return controlled_creatures[(int)Mathf.Floor (Random.Range (0, controlled_creatures.Length))];
	}

	private void call_spawn_creature() {
		StartCoroutine(spawn_creature_coroutine());
	}

	private IEnumerator spawn_creature_coroutine() {
		Vector3 spawn_position = choose_spawn_position();
		GameObject creature_type = choose_creature_type();

		GameObject spawn_flashing = (GameObject) Instantiate (this.spawn_animation, spawn_position, Quaternion.identity);
		Destroy(spawn_flashing, this.spawn_animation_duration);
		yield return new WaitForSeconds(this.spawn_animation_duration);

		GameObject spawn_creature = (GameObject) Instantiate(creature_type, spawn_position, Quaternion.identity);
//		spawn_creature.transform.parent = this.transform.parent;
		spawn_creature.GetComponent<SpawnControlledElement>().controller = this;
		active_creatures.Add (spawn_creature.GetComponent<SpawnControlledElement>());
//		highscore.register_minotar (spawn_creature.transform);
//		GameObject.FindGameObjectWithTag ("Player").GetComponent<CrowdController>().register_minotar(spawn_creature.transform);
	}

	private void register_creature(SpawnControlledElement creature) {
		active_creatures.Add (creature);
	}

	public void unregister_creature(SpawnControlledElement creature){
		active_creatures.Remove (creature);
	}

	public void on_reset() {
		foreach (SpawnControlledElement spawned_creature in active_creatures) {
			GameObject.Destroy(spawned_creature.gameObject, 0.0f);
		}
		Debug.Log ("spawner reset");
		start_spawning();
	}

	public void start_spawning() {
		// InvokeRepeating("call_spawn_creature", 1.0f, spawn_interval);
		call_spawn_creature();
	}
	
	public void stop_spawning() {
		// CancelInvoke("call_spawn_creature");
	}

	public void send_game_over_message() {
		foreach(SpawnControlledElement creature in active_creatures) {
			creature.GetComponent<MinotarAI>().wins();
		}
	}
}