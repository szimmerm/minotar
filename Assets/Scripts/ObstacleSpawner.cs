using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {

	public float spawn_delay;
	public float start_time;
	public float delay_between_patterns;
	public Vector2 grid_offset;

	private float cube_size = 41;
	private GameObject[] blocs;
	private int max_size;	
	private PathfindingManager pathfinder;

	[SerializeField]
	private bool trying_to_restart = false;

	void Awake() {
		pathfinder = GameObject.FindGameObjectWithTag("Pathfinder").GetComponent<PathfindingManager>();
		align_blocs();
		disable_all_patterns();
	}	

	// Use this for initialization
	void Start () {
		initialize_pattern();
		InvokeRepeating("spawn_bloc", start_time, spawn_delay);	
	}

	private void disable_current_pattern() {
		foreach(GameObject bloc in blocs) {
			bloc.SetActive(false);
		}
	}

	private void disable_all_patterns() {
		foreach (Transform pattern_container in this.transform) {
			pattern_container.gameObject.SetActive (true);
			foreach(Transform bloc in pattern_container) {
				bloc.gameObject.SetActive (false);
			}
		}
	}

	private void initialize_pattern() {
		Transform selected_pattern = this.transform.GetChild (Mathf.FloorToInt (Random.Range (0, this.transform.childCount)));
		this.max_size = selected_pattern.childCount;
		blocs = new GameObject[this.max_size];
		for(int i = 0; i < this.max_size ; i++) {
			blocs[i] = selected_pattern.GetChild(i).gameObject;
		}
	}

	public void align_blocs() {
		foreach(Transform pattern in this.transform) {
			foreach(Transform bloc in pattern) {
				bloc.position = align_position(bloc.position - (Vector3)grid_offset) + (Vector3)grid_offset; 
			}
		}	
	}

	private float align_value(float orig) {
		return Mathf.Round (orig/cube_size) * cube_size;
	}

	private Vector3 align_position(Vector3 orig) {
		return new Vector3(align_value(orig.x), align_value(orig.y), this.transform.position.z);
	}

	private GameObject choose_bloc() {
		// unsafe quand le tableau est vide
		int index = Mathf.FloorToInt (Random.Range (0, max_size));
		GameObject chosen_bloc = blocs[index];
		blocs[index] = blocs[--max_size];
		blocs[max_size] = chosen_bloc;
		return chosen_bloc;
	}

	private IEnumerator wait_then_reset_pattern(float timer) {
		CancelInvoke("spawn_bloc");
		trying_to_restart = true;
		yield return new WaitForSeconds(timer);
		if (trying_to_restart) {
			disable_current_pattern();
			initialize_pattern();
			pathfinder.on_reset ();	
			InvokeRepeating("spawn_bloc", spawn_delay, spawn_delay);
			trying_to_restart = false;
		} else {
			Debug.Log ("reset aborted");
		}
	}

	private void spawn_bloc() {
		if (max_size > 0) {
			GameObject bloc = choose_bloc();
			bloc.SetActive (true);
			pathfinder.add_blocker(bloc.transform.position);
		}
		else {
			StartCoroutine(wait_then_reset_pattern(delay_between_patterns));
		}
	}
	
	public void restart() {
		disable_current_pattern();
		initialize_pattern();
		InvokeRepeating("spawn_bloc", spawn_delay, spawn_delay);	
	}

	public void stop_spawning() {
		CancelInvoke("spawn_bloc");
		trying_to_restart = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
