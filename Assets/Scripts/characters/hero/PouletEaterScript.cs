using UnityEngine;
using System.Collections;

public class PouletEaterScript : MonoBehaviour {
	public int poulet_health;
	public float poulet_score;
	public int vin_health;
	public float vin_score;

	private HealthScript player_health;
	private HighScoreScript score;
	private NauseeScript nausee;

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.root.tag == "Item") {
			string fullname = other.transform.root.name;
			char[] delimiters = {'('};
			switch (fullname.Split(delimiters)[0]) {
				case "Poulet":
					player_health.add_health(poulet_health);
					score.add_score(poulet_score);
					nausee.add_nausee(1);
					Destroy(other.transform.root.gameObject);
					break;
				case "Vin":
					player_health.add_health (vin_health);
					score.add_score (vin_score);
					nausee.add_nausee(2);
					Destroy(other.transform.root.gameObject);
					break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		player_health = GetComponentInChildren<HealthScript>();
		score = GameObject.FindGameObjectWithTag("GameController").GetComponent<HighScoreScript>();
		nausee = GetComponentInChildren<NauseeScript>();
	}
}