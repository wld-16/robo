using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : StateMachineBehaviour
{

    NavMeshAgent navMeshAgent;
    RoboBehaviour roboBehaviour;
    globalVars gV;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent =  animator.GetComponent<NavMeshAgent>();
        roboBehaviour = animator.GetComponent<RoboBehaviour>();
        gV = animator.GetComponent<globalVars>();

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        Vector3 follow = gV.destination;
        if (follow != null)
        {
            var position = follow;
            Vector3 directionVector = position - animator.transform.position;
            navMeshAgent.SetDestination(position - (directionVector.normalized * roboBehaviour.stopBefore));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (navMeshAgent.destination != gV.destination)
        {
            var position = gV.destination;
            Vector3 directionVector = position - animator.transform.position;
            navMeshAgent.SetDestination(position - (directionVector.normalized * roboBehaviour.stopBefore));
           
        }
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (gV.destination == gV.roboOrigin)
                        animator.SetTrigger("originReached");
                    else
                        animator.SetTrigger("targetReached");
                }
            }
        }
    }
}
