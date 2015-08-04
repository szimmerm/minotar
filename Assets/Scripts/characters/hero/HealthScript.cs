﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int base_health = 10;
	public int current_health;
	public float flash_frames = 30;
	public Color flash_color;
	public float invincibility_timer = 0.5f;
	
	public Slider health_slider; //a changer pour plus que ça soit public, ca
	public Transform skelt_transform;

	private SpriteRenderer sprite_renderer;
	private GameControllerScript game_controller;

	private MovementScript move;

	private bool invulnerable = false;
	private ParticleSystem dash_particles;

	void Awake() {
		this.sprite_renderer = transform.root.GetComponentInChildren<SpriteRenderer>();
		this.game_controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControllerScript>();
		move = transform.root.GetComponentInChildren<MovementScript>();
		this.dash_particles = transform.root.GetComponentInChildren<ParticleSystem>();
		init_values();
	}

	private void init_values() {
		this.current_health = base_health;
		init_health_slider();
	}

	private void init_health_slider() {
		this.health_slider.maxValue = this.base_health;
		this.health_slider.value = this.current_health;
	}

	public void OnTriggerStay2D(Collider2D collision_data) {
		if(!this.invulnerable && collision_data.tag == "Minotar") {
			take_damage();
		}
	}

	public void receive_damage() {
		if (!this.invulnerable) {
			take_damage();
		}
	}

	private void take_damage() {
		this.current_health--;
		this.health_slider.value = this.current_health;
		if(this.current_health > 0) {
			StartCoroutine(flash_coroutine());
		}
		else {
			on_death();
		}
	}

	IEnumerator flash_coroutine() {
		if (!this.invulnerable) {
			this.invulnerable = true;
			Color original_color = sprite_renderer.color;
			sprite_renderer.color = flash_color;

			yield return new WaitForSeconds(flash_frames / 120);

			for(int i = (int) Mathf.Floor (flash_frames / 2); i < flash_frames; i++) {
				sprite_renderer.color = Color.Lerp(flash_color, original_color, i / flash_frames);
				yield return null;
			}

			sprite_renderer.color = original_color;
			this.invulnerable = false;
		}
	}

	void on_death() {
		game_controller.game_over();
		move.stop ();
		move.should_update_speed = false;
		move.stop ();
		dash_particles.Stop ();
		dash_particles.Clear ();
		
		invulnerable = true;
		sprite_renderer.enabled = false;

		Transform skelt = Instantiate(skelt_transform);
		skelt.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
	}

	public void on_reset() {
		init_values();
		move.should_update_speed = true;
		invulnerable = false;
		dash_particles.Play ();
		sprite_renderer.enabled = true;
	}

	public void add_health(int value) {
		current_health = (current_health + value) < 5 ? (current_health + value) : 5;
		health_slider.value = this.current_health;
	}

}
