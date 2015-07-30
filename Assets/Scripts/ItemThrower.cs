using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemThrower : MonoBehaviour {

	private PathfindingManager pathfinder;
	public Transform[] blueprints;
	public float throw_timer;

	void Awake() {
		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();
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
		int index = Mathf.FloorToInt(Random.Range (0, blueprints.Length));
		Debug.Log ("valeur choisie : "+index);
		return blueprints[index];
	}

	private void throw_item() {
		Transform new_item = (Transform) Instantiate(choose_blueprint(), choose_start_position(), Quaternion.identity);
		new_item.GetComponent<MissileScript>().set_target(pathfinder.get_random_tile ());
	}
}