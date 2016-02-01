using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour {

	public Vector2 direction;
	public float max_velocity_factor = 1;
	public float max_velocity;
	public float acceleration;

	private Rigidbody2D body;

	void Awake() {
		body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		update_speed();
	}

	public void stop() {
		body.velocity = Vector3.zero;
		direction = Vector2.zero;
	}

	private void update_speed() {
		if (direction.sqrMagnitude < 0.0001) {
			stop();
		} else {
			Vector2 base_direction = Vector2.ClampMagnitude (direction, 1.0f);
			body.AddForce (acceleration * base_direction);
			body.velocity = Vector3.ClampMagnitude (body.velocity, max_velocity * max_velocity_factor);
		}
	}
}