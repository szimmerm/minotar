using UnityEngine;
using System.Collections;

public class CrowdController : MonoBehaviour {
	public bool active = true;

	[SerializeField] private float crowd_value;
	public float taunt_cap;
	public float max_crowd_value;
	public float taunt_score_value;
	public float taunt_duration;
	private bool can_taunt = true;
	
	private bool low_crowd = true;
	private AudioSource sound_source;
	public AudioClip low_crowd_sound;
	public AudioClip high_crowd_sound;
	public AudioClip applause_sound;

	public bool crowd_active = true;
	private GameControllerScript game_controller;
	private HighScoreScript high_score;
	private MinotarPerception perception;

	private PlayerController player; // a modifier

	void Awake() {
		crowd_value = 0;
	}

	// Use this for initialization
	void Start() {
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
		player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			update_crowd_value();

			if ((low_crowd && crowd_value > taunt_cap) || (!low_crowd && crowd_value < taunt_cap)) {
				switch_sound();
			}
		}
	}

	public void on_reset() {
		active = true;
		crowd_value = 0;
		sound_source.clip = low_crowd_sound;
		sound_source.Play ();
		low_crowd = true;
		crowd_active = true;
	}

	public void on_gameover() {
		StopCoroutine(taunt_coroutine());
		active = false;
	}

	private void update_crowd_value() {
		float new_value = perception.detected ? Time.deltaTime  : 0;
		crowd_value = Mathf.Min (max_crowd_value, crowd_value + new_value);
	}

	public bool taunt_checking() {
		return (crowd_value >= taunt_cap) && can_taunt;
	}
	
	public IEnumerator taunt_coroutine() {
		high_score.add_score(taunt_score_value);
		transform.root.GetComponentInChildren<Animator>().SetBool ("tauntFinished", false);
//		transform.root.GetComponentInChildren<Animator>().SetTrigger ("tauntTrigger");
		MinotarController minotar_controller = GameObject.FindGameObjectWithTag("Minotar").GetComponent<MinotarController>();
		minotar_controller.on_taunt_start();

		AudioSource.PlayClipAtPoint(applause_sound, transform.position);
		crowd_value = 0;
		can_taunt = false;
		yield return new WaitForSeconds(taunt_duration);
		minotar_controller.on_taunt_end();
		can_taunt = true;
		transform.root.GetComponentInChildren<Animator>().SetBool ("tauntFinished", true);
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