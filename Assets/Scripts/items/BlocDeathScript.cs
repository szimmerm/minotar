using UnityEngine;
using System.Collections;

public class BlocDeathScript : MonoBehaviour {

	private Transform parent_transform;
	public Transform corpse;
	
	void on_death() {
		if (corpse != null) {
			Transform skelt = Instantiate(corpse);
			skelt.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
			Debug.Log ("pouetpouet");
		}

		GameObject.FindGameObjectWithTag("Pathfinder").GetComponent<PathfindingManager>().remove_node (transform.position);
		transform.parent.gameObject.SetActive(false);
	}

	public void set_parent(Transform parent) {
		parent_transform = parent;
	}
}
