using UnityEngine;
using System.Collections;

public class ObjectValues : MonoBehaviour {

	public Vector2 direction;
	public bool should_update_direction = true;
	public bool alive = true;
	public bool attacking = false;

	public void on_reset() {
		alive = true;
		should_update_direction = true;
	}
}
