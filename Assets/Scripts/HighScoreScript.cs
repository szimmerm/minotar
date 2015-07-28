using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreScript : MonoBehaviour {

	public int startTime;
	public float current_score;
	public Text score_HUD;

	public float highscore;
	public Text highscore_HUD;

	public bool score_paused = false;
	public int score_factor = 100;

	private Transform player;
	private GamepadControlled player_controller;
	private Transform minotar = null;

	public float score_distance;

	public float taunt_score_value;
	public float crowd_volume_cap;

	private AudioSource crowd_audio;
	private bool low_crowd = true;
	public AudioClip quiet_crowd;
	public AudioClip agitated_crowd;

	// Use this for initialization
	void Start () {
		current_score = 0;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		player_controller = player.GetComponent<GamepadControlled>();
		crowd_audio = GetComponentInChildren<AudioSource>();
	}
	
	private float compute_score(float dt, float distance) {
		//return (dt * score_factor) / distance;
		return (distance < score_distance) ? (dt*score_factor) : 0;
	}

	private float compute_public(float dt, float distance) {
		return (distance < score_distance) ? 100*dt : 0;
	}

	private void update_score() {
		if (minotar != null) {
			float dt = Time.deltaTime;
			float distance = ((Vector2)player.position - (Vector2)minotar.position).magnitude;
			current_score += compute_score(dt, distance);
			player_controller.add_public_value(compute_public(dt, distance));
			score_HUD.text = ""+((int)Mathf.Floor (current_score));
		}
	}

	public void pause_score() {
		score_paused = true;
	}

	private void update_highscore() {
		if(current_score > highscore) {
			highscore = current_score;
			highscore_HUD.text = ""+((int) highscore);
		}
	}

	public void restart() {
		update_highscore();
		current_score = 0;
		score_paused = false;
		minotar = null;
	}

	public void register_minotar(Transform candidate) {
		minotar = candidate;
	}

	public void score_taunt() {
		current_score += taunt_score_value;
	}

	// Update is called once per frame
	void Update () {
		if(!score_paused) {
			update_score();
		}
	}

	public void receive_crowd_value(float crowd_value) {
		if (!score_paused) {
			if ((crowd_value - crowd_volume_cap) < -0.01 && !low_crowd)	{
				low_crowd = true;
				crowd_audio.clip = quiet_crowd;
				crowd_audio.Play ();
			}
			if (crowd_value - crowd_volume_cap >= -0.01 && low_crowd) {
				low_crowd = false;
				crowd_audio.clip = agitated_crowd;
				crowd_audio.Play ();
			}
		}
	}
}
