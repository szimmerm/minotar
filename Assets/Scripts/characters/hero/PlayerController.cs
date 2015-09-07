using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementTools))]
[RequireComponent (typeof(MovementScript))]


public class PlayerController : MonoBehaviour {

	public enum States {walk, dash, taunt, stop};

	public float dash_reset_timer;
//	private ParticleSystem dash_particles;

	private MovementScript move;
	private MovementTools tools;
	private bool can_dash = true;

	private CrowdController crowd;

	public States state;

	// stuff for fast restart
	private Vector3 start_position;

	void Awake() {
//		dash_particles = GetComponent<ParticleSystem>();
		move = GetComponent<MovementScript>();
		tools = GetComponent<MovementTools>();
		crowd = GetComponent<CrowdController>();
		start_position = transform.position;
		state = States.walk;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case States.stop:
				move.stop ();
				break;
			case States.taunt:
				move.stop ();
				Debug.Log ("pipipidpipioupdidpazidp");
				break;
			case States.walk:
				update_direction();
				update_taunt();
				if (Input.GetButtonDown ("Dash") && can_dash) {
					call_dash();
				}
				break;
		}
	}

	void on_reset() {
		transform.position = start_position;
		can_dash = true;
		state = States.walk;
		StopCoroutine(tools.dash_coroutine (true));
		StopCoroutine(dash_delay_coroutine());
		GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	}
	
	private void update_direction() {
		float dir_x = Input.GetAxis("Horizontal");
		float dir_y = Input.GetAxis ("Vertical");
		
		move.direction = new Vector2(dir_x, dir_y);
	}

	private void update_taunt() {
		if (Input.GetButtonDown("Taunt") && crowd.taunt_checking ()) {
			StartCoroutine(taunt_call());
		}
	}

	private IEnumerator taunt_call() {
		state = States.taunt;
		Debug.Log ("stetaunt");
		yield return StartCoroutine(crowd.taunt_coroutine ());
		state = States.walk;
		Debug.Log ("finished");
	}

	public void call_dash() {
		if(move.direction.sqrMagnitude > 0.0001f) {
			StartCoroutine(tools.dash_coroutine(true));
			StartCoroutine(dash_delay_coroutine());
		}
	}

	public void call_taunt() {
		
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