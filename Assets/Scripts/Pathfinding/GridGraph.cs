using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGraph {

	public int width;
	public int height;

	private Node[,] grid;
	private HashSet<Node> blocking_nodes;

	public GridGraph(int arg_width, int arg_height) {
		width = arg_width;
		height = arg_height;
		blocking_nodes = new HashSet<Node>();

		grid = new Node[width, height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				grid[i, j] = new Node(i, j);
			}
		}
	}

	private void reset_nodes() {
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				grid[i, j].reset ();
			}
		}
	}

	public void remove_node(Node candidate) {
		HashSet<Node> res = new HashSet<Node>();
		foreach (Node node in blocking_nodes) {
			if (node.pos_x != candidate.pos_x && node.pos_y != candidate.pos_y) {
				res.Add (node);
			}
		}
		blocking_nodes = res;
	}

	private void put_node(Node node, List<Node> res) {
		res.Add(node);
	}

	private List<Node> get_neighbors(Node node) {
		List<Node> res = new List<Node>();
		if (node.pos_x > 0) {
			put_node(grid[node.pos_x - 1, node.pos_y], res);
		}
		if (node.pos_x < width-1) {
			put_node(grid[node.pos_x+1, node.pos_y], res);
		}
		if (node.pos_y > 0) {
			put_node(grid[node.pos_x, node.pos_y-1], res);
		}
		if (node.pos_y < height-1) {
			put_node(grid[node.pos_x, node.pos_y+1], res);
		}

		return res;
	}

	public void build_bfs(Node root) {
		string res = "blocking nodes :";
		foreach(Node node in blocking_nodes) {
			res += " ("+node.pos_x+" ; "+node.pos_y+")";
		}
//		Debug.Log (res);

		reset_nodes();
		root.is_root = true;
		Queue<Node> stack = new Queue<Node>();
		stack.Enqueue(root);

		while(stack.Count != 0) {
			Node current = stack.Dequeue ();
			List<Node> neighbors = get_neighbors(current);
			foreach(Node next in neighbors) {
				if (next.parent == null && !blocking_nodes.Contains (next)) {
					next.parent = current;
					stack.Enqueue (next);
				}
			}
		}
	}

	private void check_path(List<Node> path) {
		foreach(Node node in path) {
			if (blocking_nodes.Contains(node)) {
				Debug.LogError ("le chemin contient une case invalide !!");
				return;
			}
		}

//		Debug.Log ("chemin valide");
	}

	public List<Node> get_path_to_root(Node start) {
//		Debug.Log (start);
		if (start == null) {
			return new List<Node>();
		}
		
		Node current = start;
		List<Node> res = new List<Node>();
		while(!current.is_root) {
			res.Add (current);
			current = current.parent;
		}
		res.Add (current); // on rajoute la racine
//		check_path(res);
		return res;
	}

	public Node get_node_from_position(int x, int y) {
		return grid[x, y];
	}

	public void add_blocking_node(Node node) {
		blocking_nodes.Add (grid[node.pos_x, node.pos_y]);
//		Debug.LogError ("setting "+node.pos_x+" ; "+node.pos_y+" as infranchissable");
	}

	public Node get_random_node(){
		Node choice;
		do {
			int x = Mathf.FloorToInt (Random.Range (0, width));
			int y = Mathf.FloorToInt(Random.Range (0, height));
//			Debug.Log ("noeud choisi : "+x+" ; "+y);
			choice = grid[x, y];
		} while (blocking_nodes.Contains(choice));

		return choice;
	}

	public void on_reset(){
		blocking_nodes = new HashSet<Node>();
	}
}
