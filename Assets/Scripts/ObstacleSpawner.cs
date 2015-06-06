using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {

	public float spawn_delay;

	private float cube_size = 41;
	private GameObject[] blocs;
	private int max_size;	

	void Awake() {
		initialize_pattern();
		align_blocs();
	}	

	// Use this for initialization
	void Start () {
		InvokeRepeating("spawn_bloc", spawn_delay, spawn_delay);	
	}

	private void initialize_pattern() {
		Transform selected_pattern = this.transform;

		this.max_size = selected_pattern.childCount;
		blocs = new GameObject[this.max_size];
		for(int i = 0; i < this.max_size ; i++) {
			blocs[i] = selected_pattern.GetChild(i).gameObject;
			blocs[i].SetActive (false);
		}
	}

	private void align_blocs() {
		for(int i = 0 ; i < blocs.Length; i++) {
			blocs[i].transform.position = align_position(blocs[i].transform.position);
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

	private void spawn_bloc() {
		if (max_size > 0) {
			choose_bloc().SetActive (true);
		}
	}
	
	public void restart() {
		initialize_pattern();
		InvokeRepeating("spawn_bloc", spawn_delay, spawn_delay);	
	}

	public void stop_spawning() {
		CancelInvoke("spawn_bloc");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
