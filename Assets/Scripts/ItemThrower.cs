using UnityEngine;
using System.Collections;

public class ItemThrower : MonoBehaviour {

	private PathfindingManager pathfinder;
	public Transform blueprint;

	void Awake() {
		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("throw_item", 2f, 1f);
	}

	private Vector3 choose_start_position() {
		int position_index = Mathf.FloorToInt (Random.Range (0, transform.childCount));
		return transform.GetChild (position_index).position;
	}

	private void throw_item() {
		Transform new_item = (Transform) Instantiate(blueprint, choose_start_position(), Quaternion.identity);
		new_item.GetComponent<MissileScript>().set_target(pathfinder.get_random_tile ());
	}
}