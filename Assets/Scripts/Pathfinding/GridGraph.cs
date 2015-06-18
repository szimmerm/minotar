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

	private void reset() {
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++){
				grid[i, j].reset ();
			}
		}
		blocking_nodes = new HashSet<Node>();
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
		reset();
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

	public List<Node> get_path_to_root(Node start) {
		Node current = start;
		List<Node> res = new List<Node>();
		while(!current.is_root) {
			res.Add (current);
			current = current.parent;
		}
		res.Add (current); // on rajoute la racine
		return res;
	}

	public Node get_node_from_position(int x, int y) {
		return grid[x, y];
	}

	public void add_blocking_node(Node node) {
		blocking_nodes.Add (grid[node.pos_x, node.pos_y]);
		Debug.Log ("setting "+node.pos_x+" ; "+node.pos_y+"as infranchissable");
	}
}
