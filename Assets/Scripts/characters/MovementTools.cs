using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementScript))]
[RequireComponent (typeof(Rigidbody2D))]
public class MovementTools : MonoBehaviour {

	public float dash_factor;
	public float dash_time;
	private BoxCollider2D feet;

	private MovementScript move;
	private Rigidbody2D body;

	void Awake() {
		move = GetComponent<MovementScript>();
		body = GetComponent<Rigidbody2D>();
		foreach (BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			if (box.tag == "Physics") {
				feet = box;
				break;
			}
		}
	}


	/* dash tools */
	private float compute_dash_distance() {
		return this.dash_time * this.dash_factor * move.max_velocity;
	}
	
	private float compute_dash_time_to_distance(float distance) {
		return distance / (this.dash_factor * move.max_velocity);
	}

	private float compute_full_dash_time() {
		float dash_distance = compute_dash_distance();
		Vector2 offset = (feet == null) ? Vector2.zero : (Vector2)feet.transform.position + feet.offset;
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, 
		                                     move.direction, 
		                                     dash_distance, 
		                                     1 << LayerMask.NameToLayer ("Walls"));
		if (hit.collider == null) {
			return dash_time;
		} else {
			return Mathf.Min (compute_dash_time_to_distance(hit.distance), dash_time);
		}
	}
	
	public IEnumerator dash_coroutine(bool clever) {
		// original setup
		float original_drag = body.drag;
		body.drag = 0;
		CollisionDetectionMode2D original_collision_mode = body.collisionDetectionMode;
		body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		
		// dash happening
		body.velocity = move.max_velocity * dash_factor * move.direction.normalized;
		yield return new WaitForSeconds(clever ? compute_full_dash_time() : dash_time);
		body.velocity = Vector2.zero;
		
		// back to normal
		body.drag = original_drag;
		body.collisionDetectionMode = original_collision_mode;
	}
}
