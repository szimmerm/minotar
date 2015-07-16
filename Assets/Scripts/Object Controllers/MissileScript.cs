using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovingObject))]

public class MissileScript : MonoBehaviour {

	public Vector3 target;
	private ObjectValues values;

	float last_distance;

	void Awake() {
		values = GetComponent<ObjectValues>();
		last_distance = 1000000;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		float distance = (target - transform.position).sqrMagnitude;
		if (distance < 10) {
			on_target_arrival();
		}
		else if (last_distance < distance) {
			Debug.LogError ("la distance augmente !!!");		
		} else {
			last_distance = distance;
		}
	}

	void on_target_arrival() {
		values.direction = Vector3.zero;
		values.should_update_direction = false;
		Destroy(this.gameObject);
	}

	public void set_target(Vector3 target_position) {
		target = target_position;
		values.direction = (target_position - transform.position);
	}

}
