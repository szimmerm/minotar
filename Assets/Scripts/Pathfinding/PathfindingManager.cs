using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingManager : MonoBehaviour {

	public int width;
	public int height;
	public int cell_size;

	private GridGraph graph;
	private GameObject player;

	// debug
	private LineRenderer line;

	public Transform point;

	void Awake() {
		graph = new GridGraph(width, height);
		player = GameObject.FindGameObjectWithTag("Player");
		
		// debug
		line = gameObject.AddComponent<LineRenderer>();	
//		display_debuging_positions(); // a utiliser que pour debugger
	}
	
	// Update is called once per frame
	void Update () {
		graph.build_bfs(position_to_node(player.transform.position)); // trop violent, a modifier/optimiser
	}

	private void display_debuging_positions() {
		for(int i=0; i < width; i++) {
			for(int j=0; j < height; j++) {
				Vector3 spawn_position = node_to_position(new Node(i, j));
				Transform marking = (Transform) Instantiate(point, spawn_position, Quaternion.identity);
				marking.SetParent (this.transform);
			}
		}
	}

	// on utilise Node comme type de retour pour simuler les paires d'entiers
	private Node position_to_node(Vector3 position) {
		int pos_x = Mathf.FloorToInt(Mathf.Clamp ((position.x + (cell_size * width / 2))/cell_size,0 , width-1));
		int pos_y = Mathf.FloorToInt(Mathf.Clamp (((cell_size*height / 2) - position.y)/cell_size, 0, height-1));

		return graph.get_node_from_position(pos_x, pos_y);
	}

	private Vector3 node_to_position(Node node) {
		Vector3 offset = new Vector3(cell_size/2, -cell_size/2, 0);
//		Vector3 offset = Vector3.zero;
		float pos_x = node.pos_x * cell_size - (cell_size * width / 2);
		float pos_y = (node.pos_y * cell_size - (cell_size * height / 2)) * (-1);
		return new Vector3(pos_x, pos_y, 1) + offset; // on met 1 pour faciliter l'affichage sur un linerenderer, pour le debug
	}
	
	private void render_path(List<Node> path) {
		line.SetVertexCount (path.Count);

		int index = 0;
		foreach(Node node in path) {
			line.SetPosition (index++, node_to_position(node));
		}
	}

	private void print_path(List<Node> path) {
		string res = "";
		res += node_to_position(path[0]);
		for(int i = 1; i < path.Count; i++) {
			res = res + " -- "+node_to_position(path[i]);
		}

		Debug.Log (res);
	}

	public Vector3 get_next_move(Vector3 start) {
		List<Node> path = graph.get_path_to_root (position_to_node(start));
		render_path(path);
		Debug.Log ("next move : "+node_to_position(path[0]));
		return node_to_position(path[0]);
	}

	public Vector3 get_smooth_next_move(Vector3 object_position) {
		List<Node> path = graph.get_path_to_root (position_to_node(object_position));
		render_path(path);
		print_path(path);
		

		if (Physics2D.Linecast(object_position, player.transform.position, 1 << LayerMask.NameToLayer ("Walls")).collider == null) {
			Debug.Log ("no collision");
			return player.transform.position;
		}
		
		Vector3 start = node_to_position(path[0]);
		path.Reverse ();
		foreach(Node node in path) {
			Vector2 current = node_to_position(node);
			if (Physics2D.Linecast (start, current, 1 << LayerMask.NameToLayer ("Walls")).collider == null) {
				Debug.Log ("next move"+current);
				return current;
			}
		}

		return Vector3.zero; // ne devrait pas etre utilise
	}

	public void add_blocker(Vector3 position) {
		Debug.Log ("blocker coordinates : "+position);
		graph.add_blocking_node(position_to_node(position));
	}

	void OnGizmosSelected() {
		for(int i = 0; i < width; i++) {
			
		}
	}
}
