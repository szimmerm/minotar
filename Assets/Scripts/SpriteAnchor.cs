using UnityEngine;
using System.Collections;

public class SpriteAnchor : MonoBehaviour {

	public Transform sprite;

	void Awake() {
		Transform obj = (Transform)Instantiate(sprite, transform.position, Quaternion.identity);
		obj.parent = this.transform;
	}
}