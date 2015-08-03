using UnityEngine;
using System.Collections;

public class CrowdController : MonoBehaviour, IReset {


	[SerializeField] private float crowd_value;
	public float taunt_cap;
	public float max_crowd_value;
	public float taunt_score_value;
	private HighScoreScript high_score;
	
	private bool low_crowd = true;

	private MinotarPerception perception;

	private AudioSource sound_source;
	public AudioClip low_crowd_sound;
	public AudioClip high_crowd_sound;
	public AudioClip applause_sound;

	public bool crowd_active = true;
	private GameControllerScript game_controller;

	void Awake() {
		crowd_value = 0;
	}

	// Use this for initialization
	void Start() {
		ResetScript.register_in_controller (this);
		high_score = GameObject.FindGameObjectWithTag("GameController").GetComponent<HighScoreScript>();
		foreach(AudioSource candidate in GetComponentsInChildren<AudioSource>()) {
			if (candidate.name == "CrowdSound") {
				sound_source = candidate;
			}
		}
		if (sound_source == null) {
			Debug.LogError ("pas de manager de son pour le bruit de la foule");
		}
		sound_source.clip = low_crowd_sound;
		sound_source.Play ();

		game_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		perception = GetComponentInChildren<MinotarPerception>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!game_controller.is_game_over) {
			if (crowd_active) {
				update_crowd_value();
			}
	
			if (Input.GetButtonDown ("Taunt") && (crowd_value >= taunt_cap)) {
				call_taunt();
				high_score.add_score(taunt_score_value);
			}
	
			if ((low_crowd && crowd_value > taunt_cap) || (!low_crowd && crowd_value < taunt_cap)) {
				switch_sound();
			}
		}
	}

	public void on_reset() {
		crowd_value = 0;
		sound_source.clip = low_crowd_sound;
		sound_source.Play ();
		low_crowd = true;
		crowd_active = true;
	}

	private void update_crowd_value() {
		float new_value = perception.detected ? Time.deltaTime  : 0;
		crowd_value = Mathf.Min (max_crowd_value, crowd_value + new_value);
	}
	
	private void call_taunt() {
		transform.root.GetComponentInChildren<Animator>().SetTrigger ("callTaunt");
		AudioSource.PlayClipAtPoint(applause_sound, transform.position);
		crowd_value = 0;
	}

	private void switch_sound() {
		if (low_crowd) {
			sound_source.clip = high_crowd_sound;
			sound_source.Play ();
			low_crowd = false;
		}
		else {
			sound_source.clip = low_crowd_sound;
			sound_source.Play ();
			low_crowd = true;
		}
	}
}