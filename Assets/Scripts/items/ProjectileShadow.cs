using UnityEngine;
using System.Collections;

public class ProjectileShadow : MonoBehaviour {

	public float gravity = 700;
	public Vector3 speed;

	void Awake() {
		speed = new Vector3(0, 0, 0);
		gravity = 700;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void set_initial_impulse_from_time(float time_to_travel) {
		speed = new Vector3(0, 2*gravity*time_to_travel, 0);
	}

	void FixedUpdate() {
		speed -= new Vector3(0, gravity, 0) * Time.fixedDeltaTime;
//		float newY = speed.y * Time.fixedDeltaTime;
//		transform.localPosition += new Vector3(0, newY * Time.fixedDeltaTime, 0);
		transform.localPosition += speed*Time.fixedDeltaTime;
	}
} 