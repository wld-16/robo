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

        animator.ResetTrigger("originReached");
        animator.ResetTrigger("statesCorrect");
        animator.ResetTrigger("targetReached");
        animator.ResetTrigger("noHelpNeeded");
        animator.ResetTrigger("finished");
        animator.ResetTrigger("feedback");
        animator.ResetTrigger("unhappy");
        animator.ResetTrigger("findObject");

        if (gV.asked)
        {
            gV.asked = false;
            gV.feedback = false;
            roboBehaviour.SetGaze(null);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        List<GameObject> inRange = roboBehaviour.InRange("Player");
        
        foreach (GameObject player in inRange)
        {
            if (roboBehaviour.GetGazedBy() != null && roboBehaviour.GetGazedBy().transform.root == player.transform.root && roboBehaviour.lookTime >= gV.waitForGaze)
            {
                float happyValue = 1f;
                SeeEmotion seeEmotion =  roboBehaviour.GetGazedBy().GetComponentInParent<SeeEmotion>();
                if (seeEmotion != null)
                {
                    seeEmotion.emotions.TryGetValue(Emotion.Happy, out happyValue);
                    if (happyValue < gV.happyThreshold)
                    {
                        gV.destination = player.transform.position;
                        animator.SetTrigger("statesCorrect");
                    }
                }
            }
            else if(roboBehaviour.GetGazedBy() != null && roboBehaviour.GetGazedBy().transform.root == player.transform.root && roboBehaviour.lookTime <= gV.waitForGaze && roboBehaviour.lookTime != 0)
            {
                Vector3 direction = roboBehaviour.GetGazedBy().transform.position - animator.transform.position;
                Quaternion angle = Quaternion.LookRotation(direction);
                animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, angle, Time.deltaTime * 2f);
            }
            else if (roboBehaviour.GetGazedBy() != null && roboBehaviour.GetGazedBy().transform.root == player.transform.root && roboBehaviour.lookTime == 0)
            {
                animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 2f);
            }
        }
    }
}
