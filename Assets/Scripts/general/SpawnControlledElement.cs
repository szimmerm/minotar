using UnityEngine;
using System.Collections;

public class SpawnControlledElement : MonoBehaviour {

	public CreatureSpawner controller = null;

	void OnDestroy() {
		if(controller != null) {
			controller.unregister_creature(this);
		}
	}
}
