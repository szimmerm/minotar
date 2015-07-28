﻿using UnityEngine;
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

	// Use this for initialization
	void Start () {
		current_score = 0;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		player_controller = player.GetComponent<GamepadControlled>();
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

	// Update is called once per frame
	void Update () {
		if(!score_paused) {
			update_score();
		}
	}
}
