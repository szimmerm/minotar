using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreScript : MonoBehaviour {

//	public int startTime;
	public float current_score;
	public Text score_HUD;

	public float highscore;
	public Text highscore_HUD;

	public bool score_paused = false;
	public bool score_delayed = false;
	public int score_factor = 100;
	public float delay_time;
	public Transform score_sprite;

	private Transform player;
//	private GamepadControlled player_controller;
	private Transform minotar = null;

	public float score_distance;

	// Use this for initialization
	void Start () {
		current_score = 0;
		player = GameObject.FindGameObjectWithTag("Player").transform;
//		player_controller = player.GetComponent<GamepadControlled>();
	}

	private void update_score() {
		if (minotar != null) {
			if (!score_delayed && ((Vector2)player.position - (Vector2)minotar.position).magnitude < score_distance) {
				StartCoroutine(score_delay());
			}
		}
	}

	private IEnumerator score_delay() {
		score_delayed = true;
		current_score += score_factor;
		score_HUD.text = ""+(Mathf.FloorToInt (current_score));
		if (score_sprite != null) {
			Transform score_item = (Transform)Instantiate(score_sprite, player.position + new Vector3(0, 30, 0), Quaternion.identity);
			score_item.GetComponent<TextMesh>().text = score_factor.ToString ();
		}
		yield return new WaitForSeconds(delay_time);
		score_delayed = false;
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

	public void add_score(float score_value) {
		current_score += score_value;
		score_HUD.text = ""+(Mathf.FloorToInt (current_score));
	}

	// Update is called once per frame
	void Update () {
		if(!score_paused) {
			update_score();
		}
	}
}
