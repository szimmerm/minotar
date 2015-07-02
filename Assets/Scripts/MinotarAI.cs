using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ObjectValues))]
public class MinotarAI : MonoBehaviour {

	public float distance_check = 130f;
	public float dash_reload_time = 1f;
	public float blind_dash_chance = 0.3f;
	private float sqrdistance_check;

	private Transform player;
	private ObjectValues internal_values;
	private MovingObject move_script;
	private PathfindingManager pathfinder;

	private bool can_dash = false;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		internal_values = GetComponent<ObjectValues>();
		sqrdistance_check = distance_check * distance_check;
		move_script = GetComponent<MovingObject>();

		pathfinder = GameObject.FindGameObjectWithTag ("Pathfinder").GetComponent<PathfindingManager>();
//		StartCoroutine(can_dash_coroutine());
	}

	// Use this for initialization
	void Start () {
	
	}

	private float get_sign(float value) {
		if (value > 0.001) {
			return 1f;
		} else if (value < -0.001) {
			return -1f;
		} else {
			return 0f;
		}
	}

	private void go_towards_point(Vector2 point) {
		Vector2 minus = (point - (Vector2)transform.position);
		internal_values.direction = new Vector2(get_sign(minus.x), get_sign(minus.y));
	}

	private void update_direction() {
//		go_towards_point(new Vector2(0, 0));
		go_towards_point(pathfinder.get_smooth_next_move(transform.position));
//		internal_values.direction = (player.position - transform.position);
	}

	private bool is_player_far() {
		return Vector3.SqrMagnitude(player.position - this.transform.position) > sqrdistance_check;
	}

	private IEnumerator can_dash_coroutine() {
		can_dash = false;
		yield return new WaitForSeconds(dash_reload_time);
		can_dash = true;
	}

	// Update is called once per frame
	void Update () {
		if(internal_values.should_update_direction) {
			update_direction();
		}

		if (internal_values.alive && is_player_far() && this.can_dash) {
			StartCoroutine(can_dash_coroutine());
			move_script.call_wait_then_dash (Random.value > this.blind_dash_chance);
		}
	
	}
}