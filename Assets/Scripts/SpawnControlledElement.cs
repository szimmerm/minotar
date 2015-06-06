using UnityEngine;
using System.Collections;

public class SpawnControlledElement : MonoBehaviour {

	public CreatureSpawner controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		if(controller != null) {
			controller.unregister_creature(this);
		}
	}
}
