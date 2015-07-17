using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(ObjectValues))]
public class MovingObject : MonoBehaviour {
	public float max_velocity;
	public float acceleration;
	public float dash_factor;
	public float dash_time;
	public float wait_before_dash_time;
	public float dash_wait_noise;
	public bool able_to_move = true;
	
	private ObjectValues internal_values;	
	private Rigidbody2D body;
	private bool special_action = false;
	private BoxCollider2D feet;

	void Awake () {
		this.internal_values = GetComponent<ObjectValues>();
		this.body = GetComponent<Rigidbody2D>();
		foreach (BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				this.feet = box;
				break;
			}
		}
		if (this.feet == null) {
//			Debug.LogError("impossible de trouver la hitbox des pieds");
		}
	}
	
	private void move_using_direction() {
		Vector2 base_direction = Vector2.ClampMagnitude (internal_values.direction, 1.0f);
		body.AddForce (acceleration * base_direction);
		body.velocity = Vector3.ClampMagnitude (body.velocity, max_velocity);
	}

	private void stop_object() {
		body.velocity = Vector3.zero;
	}

	private void normal_move() {
		if (internal_values.direction.sqrMagnitude < 0.0001) {
			stop_object();
		}
		else {
			move_using_direction();
		}
	}
	
	public void call_wait_then_dash(bool should_update_direction) {
		if(!special_action) {
			StartCoroutine(wait_then_dash_coroutine(should_update_direction));
		}
	}

	public void call_dash() {
		if(!special_action && internal_values.direction.sqrMagnitude > 0.0001f) {
			StartCoroutine(dash_coroutine());
		}
	}

	private float compute_dash_distance() {
		return this.dash_time * this.dash_factor * this.max_velocity;
	}

	private float compute_dash_time_to_distance(float distance) {
		return distance / (this.dash_factor * this.max_velocity);
	}

	private float compute_full_dash_time() {
		float dash_distance = compute_dash_distance();
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + feet.offset, 
											internal_values.direction, 
											dash_distance, 
											1 << LayerMask.NameToLayer ("Walls"));
		if (hit.collider == null) {
			return dash_time;
		} else {
			return Mathf.Min (compute_dash_time_to_distance(hit.distance), dash_time);
		}
	}

	private IEnumerator wait_then_dash_coroutine(bool should_update_direction) {
		special_action = true;
		stop_object();
		if (!should_update_direction) {
			internal_values.should_update_direction = false;
		}
		yield return new WaitForSeconds(wait_before_dash_time - dash_wait_noise + Random.Range (0, 2*dash_wait_noise));
		internal_values.attacking = true;
		yield return StartCoroutine(dash_coroutine());
		internal_values.attacking = false;
		if (!should_update_direction) {
			internal_values.should_update_direction = true;
		}		
	}

	private IEnumerator dash_coroutine() {
		// original setup
		special_action = true;
		float original_drag = body.drag;
		body.drag = 0;
		CollisionDetectionMode2D original_collision_mode = body.collisionDetectionMode;
		body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		internal_values.should_update_direction = false;

		// dash happening
		Vector2 dash_direction = internal_values.direction.normalized;
		body.velocity = max_velocity * dash_factor * dash_direction;
		yield return new WaitForSeconds(compute_full_dash_time());
		body.velocity = Vector2.zero;

		// back to normal
		internal_values.should_update_direction = true;
		body.drag = original_drag;
		body.collisionDetectionMode = original_collision_mode;
		special_action = false;
	}

	void FixedUpdate () {
		if (!able_to_move) {
			stop_object();
		}
		else if (!special_action) {
			normal_move();
		}
	}

	void on_reset() {
		special_action = false;
	}
}
