using UnityEngine;
using System.Collections;

public class DebugObstacle : MonoBehaviour {
	private PathfindingManager pathfinder;
	
	void Start() {
		pathfinder = GameObject.FindGameObjectWithTag("Pathfinder").GetComponent<PathfindingManager>();
		foreach(Transform child in this.transform) {
			if (child != this.transform) pathfinder.add_blocker(child.position);
		}
	}	
	
}
