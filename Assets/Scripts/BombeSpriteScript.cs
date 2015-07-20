using UnityEngine;
using System.Collections;

public class BombeSpriteScript : MonoBehaviour {

	public void on_explode() {
		transform.root.GetComponent<MissileScript>().apply_explosion_damages();
	}

	public void on_finished() {
		transform.root.GetComponent<MissileScript>().make_me_die();
	}

}
