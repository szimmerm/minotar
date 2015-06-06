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


	// Use this for initialization
	void Start () {
		current_score = 0;
	}
	
	private void update_score() {
		current_score += Time.deltaTime * score_factor;
		score_HUD.text = ""+((int)Mathf.Floor (current_score));
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
	}

	// Update is called once per frame
	void Update () {
		if(!score_paused) {
			update_score();
		}
	}
}
