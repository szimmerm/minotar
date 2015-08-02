using UnityEngine;
using System.Collections;

public class PouletEaterScript : MonoBehaviour {

	private HealthScript player_health;

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Poulet") {
			player_health.add_health(1);
			Destroy(other.transform.root.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		player_health = GetComponent<HealthScript>();
	}
}