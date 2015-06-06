using UnityEngine;
using System.Collections;

public class OrientationScript : MonoBehaviour {

	private ObjectValues internal_values;
	private Animator character_animator;

	// Use this for initialization
	void Start () {
		internal_values = GetComponent<ObjectValues>();
		character_animator = GetComponent<Animator>();
	}

	private static Vector3 set_x (Vector3 vect, float value) {
		return new Vector3(value, vect.y, vect.z);
	}

	// TODO : crado d'utiliser 4 triggers, refactoriser ca sur un int
	private void set_orientation() {
		Vector2 direction = internal_values.direction;
		if (Mathf.Abs (Mathf.Abs (direction.x) - Mathf.Abs (direction.y)) < 0.1) {
			return;
		}

		if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
			// mouvement majoritairement horizontal
			if (direction.x > 0) {
				character_animator.SetTrigger ("go right");
				transform.localScale = set_x(transform.localScale, -1);
			} else if (direction.x < 0) {
				character_animator.SetTrigger ("go left");
				transform.localScale = set_x(transform.localScale, 1);
			}
		} else {
			// mouvement majoritairement vertical
			if (direction.y > 0) {
				character_animator.SetTrigger ("go up");
				transform.localScale = set_x(transform.localScale, 1);
			} else if (direction.y < 0) {
				character_animator.SetTrigger ("go down");
				transform.localScale = set_x(transform.localScale, 1);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		set_orientation();
	}
}