using UnityEngine;
using System.Collections;

public class PouletProjectileScript : MissileScript {

	protected override void on_target_arrival() {
		stop_object();

		// do stuff
		GetComponentInChildren<BoxCollider2D>().enabled = true;
		GetComponentInChildren<Animator>().SetTrigger ("landing"); //dirty coding !
	}
}
