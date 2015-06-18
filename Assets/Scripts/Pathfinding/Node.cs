using UnityEngine;
using System.Collections;

public class Node {

	public Node parent;
	public bool is_root;
	public int pos_x;
	public int pos_y;

	public Node(int x, int y) {
		pos_x = x;
		pos_y = y;
		is_root = false;
		parent = null;
	}

	public void reset() {
		is_root = false;
		parent = null;
	}
}
