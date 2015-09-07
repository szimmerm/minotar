using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int base_health = 10;
	public int current_health;
	public Transform corpse;

	public float invincibility_time = 2f;
	public float blink_time = 0.5f;
	
	public Slider health_slider; //a changer pour plus que ça soit public, ca

	private SpriteRenderer sprite_renderer;
	private GameControllerScript game_controller;
	private MovementScript move;
	private bool invulnerable = false;

	/* signals */
	void Awake() {
		game_controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControllerScript>();
		move = transform.root.GetComponentInChildren<MovementScript>();
		init_values();
	}

	void Start() {
		sprite_renderer = transform.root.GetComponentInChildren<SpriteRenderer>();
	}

	void OnTriggerStay2D(Collider2D collision_data) {
		if(!invulnerable && collision_data.tag == "Minotar") {
			take_damage(1);
		}
	}
	
	void on_reset() {
		init_values();
		invulnerable = false;
		sprite_renderer.enabled = true;
	}
	
	/* external actions */
	public void receive_damage() {
		if (!invulnerable) {
			take_damage(1);
		}
	}
	
	public void add_health(int value) {
		current_health = (current_health + value) < 5 ? (current_health + value) : 5;
		health_slider.value = this.current_health;
	}

	/* internal actions */
	private void take_damage(int damage_value) {
		current_health -= damage_value;
		update_slider();
		if(this.current_health > 0) {
			StartCoroutine(flash_coroutine());
		}
		else {
			on_death();
		}
	}

	IEnumerator flash_coroutine() {
		invulnerable = true;
	
		for(float num=0f; num < invincibility_time; num+=blink_time) {
			sprite_renderer.enabled = !sprite_renderer.enabled;
			yield return new WaitForSeconds(blink_time);
		}

		sprite_renderer.enabled = true;
		invulnerable = false;
	}

	void on_death() {
		game_controller.game_over();
		move.stop ();
		invulnerable = true;
		sprite_renderer.enabled = false;

		if (corpse != null) {
			Transform skelt = Instantiate(corpse);
			skelt.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
		}
	}
	
	private void init_values() {
		current_health = base_health;
		update_slider();
	}
	
	private void update_slider() {
		if (health_slider != null) {
			health_slider.maxValue = base_health;
			health_slider.value = current_health;
		}
	}

}
