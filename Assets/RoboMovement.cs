using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;
using Valve.VR.Extras;

public class RoboMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private SteamVR_GazeTracker gaze;
    private NavMeshAgent agent;

    [SerializeField]
    private float stopDistanceFromDestination = 15f;
    private Transform destination;
    private Transform origin;

    public bool stoppable = false;

    private float lookTime = 0f;
    private float lookAwayTime = 0f;
    [SerializeField]
    private float moveTime = 3f;
    [SerializeField]
    private float moveAwayTime = 5f;

    /*
     * Kernfunktionen:
     *  1. Bemerken wenn er x Sekunden lang vom Player angeguckt wurde
     *  2. Zum Player bewegen
     *  3. Zum gesuchten Objekt bewegen
     *  4. Zum Ausgangspunkt zurückbewegen
     *  5. Bewegung über das NavMesh
     *  
     * Loops:
     *      Movement:
     *          - wenn der Roboter x Sekunden angeguckt wurde
     *              -> Movement zum Objekt dass ihn angeguckt hat (Player)
     *                    -> x Sekunden auf Eingabe warten
     *                    -> Wenn keine EIngabe kam: Zurück zur Ausgangsposition
     *                    -> Wenn Eingabe kam: Zum ausgewählten Ziel bewegen
     *                          -> Wenn Ziel erreicht wurde: zurück zur Ausgangsposition
     *      Animation:
     *          - Walk Animation wenn der Roboter sich bewegt
     *          - Idle Animation wenn der Roboter sich nicht bewegt
     * 
     */

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.Log("No Rigidbody");
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("No Animator");
        gaze = GetComponent<SteamVR_GazeTracker>();
        if (gaze == null)
            Debug.Log("No SteamVR_GazeTracker");
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        origin = transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGaze();

        if (destination != null)
        {
            agent.destination = destination.position;
        }

        AnimationCheck();
    }

    void CheckForGaze()
    {
        if (gaze.isInGaze)
        {
            lookAwayTime = 0f;
            lookTime += Time.deltaTime;
        }
        else
        {
            lookTime = 0f;
            lookAwayTime += Time.deltaTime;
        }
        if (lookTime >= moveTime)
        {
            SetDestination(gaze.hmd.transform);
        }
        else if (lookAwayTime >= moveAwayTime)
        {
            SetDestination(origin);
        }
    }

    public void SetDestination(Transform dest)
    {
        if (stoppable)
            destination = dest;
    }

    private bool DestinationReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= stopDistanceFromDestination)
            {
                if (agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void AnimationCheck()
    {
        if (DestinationReached())
            anim.SetBool("Walk_Anim", false);
        else
        {
            anim.SetBool("Walk_Anim", true);
        }

    }
}
