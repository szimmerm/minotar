using UnityEngine;
using System.Collections;

public class StopMovementDuringAnimation : StateMachineBehaviour {

	private Transform player;
	private MovingObject player_controller_script;
	private CrowdController crowd_controller;
	private MinotarWalkRun minotar_controller;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform.root;
		player_controller_script = player.GetComponent<MovingObject>();
		player_controller_script.able_to_move = false;
		crowd_controller = player.GetComponent<CrowdController>();
		crowd_controller.crowd_active = false;

		GameObject minotar = GameObject.FindGameObjectWithTag("Minotar");
		minotar_controller = minotar.transform.root.GetComponentInChildren<MinotarWalkRun>();
		minotar_controller.force_run = true;
	}
	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = animator.gameObject.transform;
		player_controller_script.able_to_move = true;
		minotar_controller.force_run = false;
		crowd_controller.crowd_active = true;
	}
}
