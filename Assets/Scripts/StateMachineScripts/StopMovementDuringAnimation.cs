using UnityEngine;
using System.Collections;

public class StopMovementDuringAnimation : StateMachineBehaviour {

	private Transform player;
	private MovingObject player_controller_script;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform.root;
		player_controller_script = player.GetComponent<MovingObject>();
		player_controller_script.able_to_move = false;
	}
	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform;
		player_controller_script.able_to_move = true;
	}
}
