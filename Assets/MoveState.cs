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

        //GameObject gazedBy = roboBehaviour.GetGazedBy();
        GameObject follow = gV.followGO;
        if (follow != null)
        {
            var position = follow.transform.position;
            Vector3 directionVector = position - animator.transform.position;
            Debug.Log(Vector3.Distance(position, position - (directionVector.normalized * roboBehaviour.stopBefore)));
            navMeshAgent.SetDestination(position - (directionVector.normalized * roboBehaviour.stopBefore));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (navMeshAgent.destination != gV.followGO.transform.position)
        {
            var position = gV.followGO.transform.position;
            Vector3 directionVector = position - animator.transform.position;
            navMeshAgent.SetDestination(position - (directionVector.normalized * roboBehaviour.stopBefore));
           
        }
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log(Vector3.Distance(navMeshAgent.transform.position, gV.followGO.transform.position));
                    animator.SetTrigger("targetReached");
                }
            }
        }
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
