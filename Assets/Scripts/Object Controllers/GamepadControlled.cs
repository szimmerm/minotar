using UnityEngine;
using System.Collections;


[RequireComponent (typeof(ObjectValues))]
[RequireComponent (typeof(MovingObject))]
public class GamepadControlled : MonoBehaviour {

	public int max_dash;
	public float dash_timer = 3f;
	private int dash_left;

	private ObjectValues internal_values;
	private MovingObject move_script;	
	private bool can_dash = true;
	private ParticleSystem dash_particles;
	private HighScoreScript score_controller;

	public float public_value;
	public float max_public;

	public AudioClip applause_sound;
	
	void Awake () {
		internal_values = GetComponent<ObjectValues>();
		move_script = GetComponent<MovingObject>();
		dash_particles = GetComponentInChildren<ParticleSystem>();
//		score_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<HighScoreScript>();
	}
	
	// Use this for initialization
	void Start () {
	}
	
	void update_direction() {
		float dir_x = Input.GetAxis("Horizontal");
		float dir_y = Input.GetAxis ("Vertical");
		
		internal_values.direction = new Vector2(dir_x, dir_y);
	}

	private IEnumerator dash_reset_coroutine() {
		dash_particles.Stop();
		dash_particles.Clear ();
		can_dash = false;
		yield return new WaitForSeconds(dash_timer);
		can_dash = true;
		dash_particles.Play();
	}

	private void call_dash_reset() {
		StartCoroutine(dash_reset_coroutine());
	}

	// Update is called once per frame
	void Update () {
		if(internal_values.should_update_direction) {
			update_direction();
		}
		if (Input.GetButtonDown ("Dash") && can_dash) {
			move_script.call_dash ();
			call_dash_reset();
		}
	}

	public void on_reset() {
		can_dash = true;
	}
}
