using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovingObject))]

public class MissileScript : MonoBehaviour {

	public Vector3 target;
	private ObjectValues values;
	private GameObject player;

	private bool active = true;

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

	void on_target_arrival() {
		active = false;
		values.direction = Vector2.zero;
		GetComponentInChildren<Animator>().SetTrigger ("Explode");
		GetComponentInChildren<ProjectileShadow>().enabled = false;
	}

	public void apply_explosion_damages() {
		if ((transform.position - player.transform.position).sqrMagnitude < 30*30) {
			player.GetComponent<HealthScript>().receive_damage ();
		}
	}

	public void set_target(Vector3 target_position) {
		target = target_position;
		if (values == null) {
			values = GetComponent<ObjectValues>();
		}
		values.direction = (target_position - transform.position);
	}

	public void make_me_die() {
		Destroy(this.gameObject);
	}

}
