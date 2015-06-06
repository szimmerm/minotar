using UnityEngine;

[ExecuteInEditMode]
public class PixelDensityCamera : MonoBehaviour {
	
	public float pixelsToUnits = 100;
	private Camera myCamera;

	void Start () {
		myCamera = GetComponent<Camera>();
	}
	
	void Update () {
		myCamera.orthographicSize = Screen.height / pixelsToUnits / 2;
	}
}
