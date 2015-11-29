using UnityEngine;
using System.Collections;

public class SetParent : MonoBehaviour {
	void Awake() {
		gameObject.BroadcastMessage("set_parent", this.transform);
	}
}
