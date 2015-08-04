using UnityEngine;
using System.Collections;

public class DrawCircleCollider : MonoBehaviour {

	private LineRenderer line;
	private CircleCollider2D hitbox;
	public int dots;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		hitbox = GetComponent<CircleCollider2D>();
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.sortingLayerName = "Foreground";
		line.sortingOrder = 10;

		draw_circle();
	}

	private void draw_circle() {
		line.SetVertexCount (dots+1);
		float radius = hitbox.radius;
		for(int i=0 ; i<= dots; i++) {
			line.SetPosition (i, new Vector3(radius * Mathf.Cos(2*i*Mathf.PI/(float)dots), 0, radius * Mathf.Sin(2*i*Mathf.PI/(float)dots)));
		}
	}
	
	// Update is called once per frame
	void Update () {
//		draw_circle();
	}
}
