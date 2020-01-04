using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackState : StateMachineBehaviour
{
    globalVars gV;
    Looker looker;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gV = animator.GetComponent<globalVars>();
        looker = gV.interactionCanvas.GetComponent<Looker>();

        gV.feedback = true;

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        animator.SetTrigger("finished");
    }

}
