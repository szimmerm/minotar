using UnityEngine;
using System.Collections;

// codage :
// droite = 1
// gauche = 2
// haut = 3
// bas = 4

[RequireComponent (typeof(MovementScript))]
public class OrientationScript : MonoBehaviour {

	private MovementScript move;
	private Animator character_animator;
	private Rigidbody2D body;

	public float sqrSpeed;

	// Use this for initialization
	void Start () {
		move = GetComponent<MovementScript>();
		character_animator = GetComponentInChildren<Animator>();
		body = GetComponent<Rigidbody2D>();
	}

	private static Vector3 set_x (Vector3 vect, float value) {
		return new Vector3(value, vect.y, vect.z);
	}

	private void set_orientation() {
		Vector2 direction = move.direction;

		if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y) || (direction.y * direction.y < 0.8)) {
			// mouvement majoritairement horizontal
			if (direction.x > 0) {
				character_animator.SetInteger ("direction", 1);
				transform.localScale = set_x(transform.localScale, -1);
			} else if (direction.x < 0) {
				character_animator.SetInteger ("direction", 2);
				transform.localScale = set_x(transform.localScale, 1);
			} 
		} else {
			// mouvement majoritairement vertical
			if (direction.y > 0) {
				character_animator.SetInteger ("direction", 3);
				transform.localScale = set_x(transform.localScale, 1);
			} else if (direction.y < 0) {
				character_animator.SetInteger ("direction", 4);
				transform.localScale = set_x(transform.localScale, 1);
			}
		}
	}
	
	void set_speed() {
		character_animator.SetFloat ("sqrSpeed", body.velocity.sqrMagnitude);
	}

	// Update is called once per frame
	void Update () {
		set_orientation();
		set_speed();
	}
}