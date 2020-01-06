using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackState : StateMachineBehaviour
{
    globalVars gV;
    Looker looker;
    RoboBehaviour roboBehaviour;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gV = animator.GetComponent<globalVars>();
        looker = gV.interactionCanvas.GetComponent<Looker>();
        roboBehaviour = animator.GetComponent<RoboBehaviour>();

        gV.feedback = true;

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        looker.StartFeedbackDialog();

        float happyValue = 1f;
        SeeEmotion seeEmotion = roboBehaviour.GetGazedBy().GetComponentInParent<SeeEmotion>();
        if (seeEmotion != null)
        {
            //seeEmotion.emotions.TryGetValue(Emotion.Happy, out happyValue);
            //Debug.Log(happyValue);
            happyValue = seeEmotion.GetEmotionMean()[2];
            Debug.Log(happyValue);
            if (happyValue < gV.happyThreshold)
            {
                gV.asked = false;
                gV.feedback = false;
            }
        }
            animator.SetTrigger("finished");
    }

}
