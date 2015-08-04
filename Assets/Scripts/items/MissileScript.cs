using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovingObject))]

public class MissileScript : MonoBehaviour {

	public Vector3 target;
	private ObjectValues values;
	protected GameObject player;

	protected bool active = true;

	float last_distance;

	void Awake() {
		values = GetComponent<ObjectValues>();
		player = GameObject.FindGameObjectWithTag ("Player");
		last_distance = 1000000;
	}

	// Use this for initialization
	void Start () {
		float max_velocity = GetComponent<MovingObject>().max_velocity;
		float travel_time = (target - transform.position).magnitude / (6*max_velocity);
		GetComponentInChildren<ProjectileShadow>().set_initial_impulse_from_time(travel_time);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		if (active) {
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
	}

	protected virtual void on_target_arrival() {
		active = false;
		values.direction = Vector2.zero;
		GetComponentInChildren<Animator>().SetTrigger ("Explode");
		GetComponentInChildren<ProjectileShadow>().enabled = false;
	}

	public void set_target(Vector3 target_position) {
		target = target_position;
		if (values == null) {
			values = GetComponent<ObjectValues>();
		}
		values.direction = (target_position - transform.position);
	}

	public virtual void on_reset() {
		Destroy(this.gameObject);
	}

	protected void stop_object() {
		// stop movement
		GetComponent<MovingObject>().stop_object();
		GetComponentInChildren<ObjectValues>().direction = Vector2.zero;
		ProjectileShadow shadowScript = GetComponentInChildren<ProjectileShadow>();
		shadowScript.enabled = false;
		shadowScript.transform.localPosition = Vector3.zero;
		active = false;
	}
}
