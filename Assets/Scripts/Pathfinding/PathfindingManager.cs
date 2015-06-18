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

	void Awake() {
		graph = new GridGraph(width, height);
		player = GameObject.FindGameObjectWithTag("Player");
		
		// debug
		line = gameObject.AddComponent<LineRenderer>();	
	}
	
	// Update is called once per frame
	void Update () {
		graph.build_bfs(position_to_node(player.transform.position)); // trop violent, a modifier/optimiser
	}

	// on utilise Node comme type de retour pour simuler les paires d'entiers
	private Node position_to_node(Vector3 position) {
		int pos_x = Mathf.FloorToInt((position.x + (cell_size * width / 2))/cell_size);
		int pos_y = Mathf.FloorToInt((cell_size*height / 2) - position.y)/cell_size;

		return graph.get_node_from_position(pos_x, pos_y);
	}

	private Vector3 node_to_position(Node node) {
		float pos_x = node.pos_x * cell_size - (cell_size * width / 2);
		float pos_y = (node.pos_y * cell_size - (cell_size * height / 2)) * (-1);
		return new Vector3(pos_x, pos_y, 1); // on met 1 pour faciliter l'affichage sur un linerenderer, pour le debug
	}
	
	private void render_path(List<Node> path) {
		line.SetVertexCount (path.Count);

		int index = 0;
		foreach(Node node in path) {
			line.SetPosition (index++, node_to_position(node));
		}
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
		

		if (Physics2D.Linecast(object_position, player.transform.position, 1 << LayerMask.NameToLayer ("Walls")).collider == null) {
			Debug.Log ("no collision");
			return player.transform.position;
		}
		
		Debug.Log ("collision hiiiii");

		//Debug.Log ("Taille du chemin : "+path.Count);
		if (path.Count <= 1) {
			return node_to_position(path[0]);
		}
		Vector3 start = node_to_position(path[0]);
		Vector3 prev = start;

		for(int i = 1; i < path.Count; i++) {
			Node node = path[i];
			Vector3 current = node_to_position(node);
			if (Physics2D.Linecast (start, current, 1 << LayerMask.NameToLayer ("Walls")).collider == null) {
				prev = current;		
				//Debug.Log ("noeud valide : "+node.pos_x+" ; "+node.pos_y);
			} else {
				Debug.Log ("next move : "+prev);
				Debug.Log ("=============!!!=============");
				return prev;
			}
		}
		Debug.Log ("next move : "+prev);
		Debug.Log ("====================");
		return prev;
	}

	public void add_blocker(Vector3 position) {
		graph.add_blocking_node(position_to_node(position));
	}
}
