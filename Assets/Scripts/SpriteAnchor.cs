using UnityEngine;
using System.Collections;

public class SpriteAnchor : MonoBehaviour {

	public Transform sprite;
	public bool disable_renderer = false;

	void Awake() {
		Transform obj = (Transform)Instantiate(sprite, transform.position, Quaternion.identity);
		obj.parent = this.transform;
		if (disable_renderer) {
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}