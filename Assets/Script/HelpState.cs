using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelpState : StateMachineBehaviour
{

    globalVars gV;
    NavMeshAgent agent;
    Looker looker;

    float waitingForSecs = 0f;
    float cancelInteraction = 10f;

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

        waitingForSecs = 0f;

        looker.StartHelpDialog();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (gV.asked)
            animator.SetTrigger("findObject");

        waitingForSecs += Time.deltaTime;
        if (waitingForSecs >= cancelInteraction)
        {
            gV.helpPanel.SetActive(false);
            gV.feedbackPanel.SetActive(false);
            gV.searchingObjPanel.SetActive(false);
            gV.destination = gV.roboOrigin;
            animator.SetTrigger("findObject");
        }
    }
}
