using UnityEngine;
using System.Collections;

public class NauseeScript : MonoBehaviour {

	private MovementScript move;
	[SerializeField] private float nausee_value = 0;
	public float nausee_gap = 3;
	private float nausee_effect;
	public int max_nausee_effect = 5;

	// Use this for initialization
	void Start () {
		move = GetComponentInChildren<MovementScript>();
		nausee_effect = 1/8f;
	}
	
	// Update is called once per frame
	void Update () {
		if (nausee_value > 0) {
			nausee_value -= Time.deltaTime;
		}

		for (int i = 0; i < 5; i++) {
			if (nausee_value <= (nausee_gap*i)) {
				move.max_velocity_factor = 1 - (1*nausee_effect*i);
				break;
			}
		}
	}

	public void add_nausee (int value) {
		nausee_value += nausee_gap * value;
	}
}
