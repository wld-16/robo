using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionState : StateMachineBehaviour
{

    globalVars gV;
    Looker looker;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gV = animator.GetComponent<globalVars>();
        looker = gV.interactionCanvas.GetComponent<Looker>(); ;

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        gV.interactionCanvas.gameObject.SetActive(true);

        gV.helpPanel.SetActive(false);
        gV.searchingObjPanel.SetActive(false);
        gV.feedbackPanel.SetActive(false);

        //Emotion Check fehlt noch
        if (!gV.asked)
            animator.SetTrigger("unhappy");
        else if (gV.asked && !gV.feedback)
        {
            looker.StartArrivalDialog();
            animator.SetTrigger("feedback");
        }       
        else if (gV.asked && gV.feedback)
        {
            animator.SetTrigger("noHelpNeeded");
            gV.destination = gV.roboOrigin;
        }
            
    }
}
