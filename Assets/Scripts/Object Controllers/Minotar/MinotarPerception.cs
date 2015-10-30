using UnityEngine;
using System.Collections;

public class MinotarPerception : MonoBehaviour{

	private Animator animator;
	public bool detected = false;

	void Awake() {
		animator = transform.root.GetComponentInChildren<Animator>();
	}

	public void OnTriggerEnter2D() {
		detected = true;
		animator.SetFloat ("State", 1.0f);
	}

	public void OnTriggerStay2D() {
	}

	public void OnTriggerExit2D() {
		animator.SetFloat ("State", 0.0f);
		detected = false;
	}

	public void on_reset() {
		animator.SetFloat ("State", 0.0f);
		detected = false;
	}
}
