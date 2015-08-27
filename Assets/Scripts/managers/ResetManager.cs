using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetManager : MonoBehaviour {
	private HashSet<GameObject> active_objects;

	void Awake() {
		active_objects = new HashSet<GameObject>();
	}

	public void register_object(GameObject obj) {
		active_objects.Add (obj);
	}
	
	public void unregister_object(GameObject obj) {
		active_objects.Remove(obj);
	}

	void on_reset() {
		foreach (GameObject obj in active_objects) {
			if (obj != null) {
				obj.BroadcastMessage("on_reset", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
