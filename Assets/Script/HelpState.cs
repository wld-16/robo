using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelpState : StateMachineBehaviour
{

    globalVars gV;
    NavMeshAgent agent;
    Looker looker;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gV = animator.GetComponent<globalVars>();
        agent = animator.GetComponent<NavMeshAgent>();
        looker = gV.interactionCanvas.GetComponent<Looker>();

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        gV.helpPanel.SetActive(true);
        gV.feedbackPanel.SetActive(false);
        gV.searchingObjPanel.SetActive(false);

        looker.StartHelpDialog();
    }

    public void SetDestination(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (gV.asked)
            animator.SetTrigger("findObject");
    }
}
