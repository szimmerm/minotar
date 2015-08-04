using UnityEngine;
using System.Collections;

public class BombeProjectileScript : MissileScript {

	public float explosion_delay;
	public float explosion_radius;
	
	protected override void on_target_arrival() {
		stop_object();

		StartCoroutine(wait_then_explode());
	}


	private IEnumerator wait_then_explode() {
		yield return new WaitForSeconds(explosion_delay);
		GetComponentInChildren<Animator>().SetTrigger ("Explode");
	}
	
	public void apply_explosion_damages() {
		if ((transform.position - player.transform.position).sqrMagnitude < explosion_radius*explosion_radius) {
			player.GetComponent<HealthScript>().receive_damage ();
		}
	}

	public void make_me_die() {
		Destroy(this.gameObject);
	}
}
