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
        }
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
            if (roboBehaviour.GetGazedBy() != null && roboBehaviour.GetGazedBy().transform.root == player.transform.root)
            {
                
                float happyValue = 1f;
                SeeEmotion seeEmotion =  roboBehaviour.GetGazedBy().GetComponentInParent<SeeEmotion>();
                if (seeEmotion != null)
                {
                    seeEmotion.emotions.TryGetValue(Emotion.Happy, out happyValue);
                    if (happyValue < 0.5f)
                    {
                        gV.destination = player.transform.position;
                        animator.SetTrigger("statesCorrect");
                    }
                }
            }
        }
    }
}
