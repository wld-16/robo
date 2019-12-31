using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    private RoboBehaviour roboBehaviour;
    private globalVars gV;
    private static readonly int Move = Animator.StringToHash("move");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        roboBehaviour = animator.GetComponent<RoboBehaviour>();
        gV = animator.GetComponent<globalVars>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        /*if (roboBehaviour.NotHappy(roboBehaviour.InRange("Player")
            .Where(GO => roboBehaviour.GetGazedBy() == GO).ToList()
            .First().GetComponent<EmotionState>()))
        {
            animator.SetTrigger(Move);
        }
        */

        List<GameObject> inRange = roboBehaviour.InRange("Player");

        foreach (GameObject player in inRange)
        {
            if (roboBehaviour.GetGazedBy().transform.root == player.transform.root)
            {
                gV.followGO = player;
                animator.SetTrigger("statesCorrect");
            }
        }
        /*
        if (roboBehaviour.InRange("Player").ToList().Where(GO =>
        {
            Debug.Log(GO.transform.name);
            if (roboBehaviour.GetGazedBy() == GO)
                return GO;
            else
                return null;
        }).ToList().Count > 0)
        {
            animator.SetTrigger(Move);
        }
        */
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
