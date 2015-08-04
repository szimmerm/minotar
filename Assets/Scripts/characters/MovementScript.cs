using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour {

	public bool should_update_speed = true;
	public Vector2 direction;
	public float max_velocity;
	public float acceleration;

	private Rigidbody2D body;

	void Awake() {
		body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if (should_update_speed) {
			update_speed();
		}
	}

	public void stop() {
		body.velocity = Vector3.zero;
	}

	private void update_speed() {
		if (direction.sqrMagnitude < 0.0001) {
			stop();
		} else {
			Vector2 base_direction = Vector2.ClampMagnitude (direction, 1.0f);
			body.AddForce (acceleration * base_direction);
			body.velocity = Vector3.ClampMagnitude (body.velocity, max_velocity);		
		}
	}
}
