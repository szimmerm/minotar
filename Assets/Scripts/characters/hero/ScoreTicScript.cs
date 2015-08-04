using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TextMesh))]
public class ScoreTicScript : MonoBehaviour {

	private TextMesh text;
	public float fade_tic_time;
	public float move_speed;

	private float fade_value = 0.1f;

	void Awake() {
		text = GetComponent<TextMesh>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(fade_out());
	}

	private IEnumerator fade_out() {
		yield return new WaitForSeconds(fade_tic_time);
		do {
			text.color = decrease_color_alpha(text.color);
			yield return new WaitForSeconds(fade_tic_time);
		} while (text.color.a > 0);
		Destroy(this.gameObject);
	}

	private Color decrease_color_alpha(Color origin) {
		float r = origin.r;
		float g = origin.g;
		float b = origin.b;
		float a = origin.a - fade_value;
		Color result = new Color(r, g, b, a);
		return result;
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.position += new Vector3(0, move_speed, 0);
	}
}
