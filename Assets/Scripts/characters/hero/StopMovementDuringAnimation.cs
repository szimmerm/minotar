using UnityEngine;
using System.Collections;

public class StopMovementDuringAnimation : StateMachineBehaviour {

	private Transform player;
	private MovementScript player_move;
	private CrowdController crowd_controller;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform.root;
		player_move = player.GetComponent<MovementScript>();
		player_move.stop ();
//		player_move.should_update_speed = false;
		crowd_controller = player.GetComponent<CrowdController>();
		crowd_controller.crowd_active = false;

	}
	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform;
//		player_move.should_update_speed = true;
		crowd_controller.crowd_active = true;
	}
}
