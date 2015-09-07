using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementTools))]
[RequireComponent (typeof(MovementScript))]


public class PlayerController : MonoBehaviour {

	public float dash_reset_timer;
//	private ParticleSystem dash_particles;

	private MovementScript move;
	private MovementTools tools;
	private bool should_update_direction = true;
	private bool can_dash = true;

	// stuff for fast restart
	private Vector3 start_position;

	void Awake() {
//		dash_particles = GetComponent<ParticleSystem>();
		move = GetComponent<MovementScript>();
		tools = GetComponent<MovementTools>();
		start_position = transform.position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(should_update_direction) {
			update_direction();
		}

		if (Input.GetButtonDown ("Dash") && can_dash) {
			call_dash();
		}
	}

	void on_reset() {
		transform.position = start_position;
		can_dash = true;
		should_update_direction = true;
		StopCoroutine(tools.dash_coroutine (true));
		StopCoroutine(dash_delay_coroutine());
		GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	}
	
	void update_direction() {
		float dir_x = Input.GetAxis("Horizontal");
		float dir_y = Input.GetAxis ("Vertical");
		
		move.direction = new Vector2(dir_x, dir_y);
	}

	public void call_dash() {
		if(move.direction.sqrMagnitude > 0.0001f) {
			StartCoroutine(tools.dash_coroutine(true));
			StartCoroutine(dash_delay_coroutine());
		}
	}

	private IEnumerator dash_delay_coroutine() {
//		dash_particles.Stop();
//		dash_particles.Clear ();
		can_dash = false;
		yield return new WaitForSeconds(dash_reset_timer);
		can_dash = true;
//		dash_particles.Play();
	}
}