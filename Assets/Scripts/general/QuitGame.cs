using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown("Quit")) {
			Application.Quit ();
		}
	}

}
