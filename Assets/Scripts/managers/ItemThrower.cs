using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemThrower : MonoBehaviour {

	private PathfindingManager pathfinder;
	public float throw_timer;
	public Transform[] blueprints;
	public int[] frequency;
	public int void_frequency;

	private Pair<Transform, int>[] internal_thrower;
	private int sum = 0;

	void Awake() {
		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();

		if (blueprints.Length == frequency.Length) {
			internal_thrower = new Pair<Transform, int>[blueprints.Length];
			for(int i=0; i<blueprints.Length; i++) {
				sum += frequency[i];
				internal_thrower[i] = new Pair<Transform, int>(blueprints[i], sum);
			}
			sum += void_frequency;
		} else {
			Debug.LogError ("le tableau d'objet et celui des poids n'ont pas la meme longueur");
			internal_thrower = new Pair<Transform, int>[0];
			sum = 0;
		}
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("throw_item", 2f, throw_timer);
	}

	private Vector3 choose_start_position() {
		int position_index = Mathf.FloorToInt (Random.Range (0, transform.childCount));
		return transform.GetChild (position_index).position;
	}

	private Transform choose_blueprint() {
		int index = sum - Mathf.FloorToInt(Random.Range (0, sum));
		for(int i=0; i<internal_thrower.Length; i++) {
			if (internal_thrower[i].snd >= index) {
				return internal_thrower[i].fst;
			}
		}
		return null;
	}

	private void throw_item() {
		Transform blueprint = choose_blueprint();
		if (blueprint != null) {
			Transform new_item = (Transform) Instantiate(blueprint, choose_start_position(), Quaternion.identity);
			new_item.GetComponent<MissileScript>().set_target(pathfinder.get_random_tile ());
		}
	}
}