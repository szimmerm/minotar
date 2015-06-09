using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(ObstacleSpawner))]
public class ObstacleSpawnerEditor : Editor {

	private ObstacleSpawner spawner;

	public void OnEnable() {
		spawner = (ObstacleSpawner) target;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if (GUILayout.Button ("Caller les patterns")) {
			spawner.align_blocs();
		}
	}

}
