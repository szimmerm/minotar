using UnityEngine;
using System.Collections;

public class NenetteController : MonoBehaviour {

	private enum State {
		walk,
	}

	
	public Vector2 directionDelay;
	public Vector2 waitDelay;
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
//				setRandomDirection(0, 2*Mathf.PI);
				StartCoroutine(waitThenMove());
			}
			break;
		}
	}

	private IEnumerator delayRoutine(float min, float max) {
		delaying = true;
		yield return new WaitForSeconds(Random.Range (min,  max));
		delaying = false;
	}

	private IEnumerator waitThenMove() {
		Debug.Log ("wait");
		move.direction = new Vector2(0, 0);
		yield return StartCoroutine(delayRoutine(waitDelay.x, waitDelay.y));
		setRandomDirection();
		Debug.Log ("move");
		yield return StartCoroutine(delayRoutine(directionDelay.x, directionDelay.y));
	}

	void setRandomAngle(float min, float max) {
		float angle = Random.Range(min, max);
		move.direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

	void setRandomDirection () {
		int value = Mathf.FloorToInt(Random.Range (0, 4));
		Vector2 direction;
		switch (value) {
			case 0:
				direction = new Vector2(1, 0);
				break;
			case 1:
				direction = new Vector2(0, -1);
				break;
			case 2:
				direction = new Vector2(-1, 0);
				break;
			case 3:
				direction = new Vector2(0, 1);
				break;
			default:
				direction = new Vector2(0, 0);
				break;
		}
		move.direction = direction;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 normal = collision.contacts[0].normal;
//		Vector2 angle;
		/*
		if (normal.x >= 0.99f) {
			angle = new Vector2(-Mathf.PI/2, Mathf.PI/2);
		} else if (normal.x <= -0.99f) {
			angle = new Vector2(Mathf.PI/2, 3*Mathf.PI / 2);
		} else if (normal.y >= 0.99f) {
			angle = new Vector2(0, Mathf.PI);
		} else {
			angle = new Vector2(Mathf.PI, 2*Mathf.PI);
		}
		setRandomDirection(normal.x, normal.y);*/
		setRandomDirection();
		StopCoroutine(waitThenMove());
		StartCoroutine(waitThenMove());
	}
}
