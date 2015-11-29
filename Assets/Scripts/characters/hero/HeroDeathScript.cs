using UnityEngine;
using System.Collections;

public class HeroDeathScript : MonoBehaviour {

	public Transform corpse;

	void on_death() {
		if (corpse != null) {
			Transform skelt = Instantiate(corpse);
			skelt.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
