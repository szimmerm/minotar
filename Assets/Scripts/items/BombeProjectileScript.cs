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
		apply_explosion_damages();
	}
	
	public void apply_explosion_damages() {
		Collider2D[] guys_arounds = Physics2D.OverlapCircleAll(
			transform.position, 
			explosion_radius,
			1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Walls"));


		foreach(Collider2D coll in guys_arounds) {
			Debug.Log ("in explostion : "+coll.transform.root.gameObject);
			HealthScript hlth = coll.transform.root.GetComponentInChildren<HealthScript>();
			if (hlth != null) hlth.receive_damage ();
		}
	}

	public void make_me_die() {
		Destroy(this.gameObject);
	}
}
