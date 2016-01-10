using UnityEngine;
using System.Collections;

public class NenetteController : MonoBehaviour {

	private enum State {
		walk,
	}

	
	public Vector2 directionDelay;
	private bool delaying = false;
	private MovementScript move;
	private State state;

	void Awake () {
		move = GetComponent<MovementScript>();
		state = State.walk;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (state)  {
		case State.walk:
			if (!delaying) {
				setRandomDirection(0, 2*Mathf.PI);
				StartCoroutine(delayRoutine());
			}
			break;
		}
	}

	private IEnumerator delayRoutine() {
		delaying = true;
		yield return new WaitForSeconds(Random.Range (directionDelay.x, directionDelay.y));
		delaying = false;
	}

	void setRandomDirection(float min, float max) {
		float angle = Random.Range(min, max);
		move.direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 normal = collision.contacts[0].normal;
		Debug.Log (normal);
		Vector2 angle;
		if (normal.x >= 0.99f) {
			angle = new Vector2(-Mathf.PI/2, Mathf.PI/2);
		} else if (normal.x <= -0.99f) {
			angle = new Vector2(Mathf.PI/2, 3*Mathf.PI / 2);
		} else if (normal.y >= 0.99f) {
			angle = new Vector2(0, Mathf.PI);
		} else {
			angle = new Vector2(Mathf.PI, 2*Mathf.PI);
		}
		setRandomDirection(normal.x, normal.y);
		StopCoroutine(delayRoutine());
		StartCoroutine(delayRoutine());
	}
}
