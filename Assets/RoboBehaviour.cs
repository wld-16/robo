using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;
using Valve.VR.Extras;

public class RoboBehaviour : MonoBehaviour
{
    private SteamVR_GazeTracker steamVR_GazeTracker;
    private Rigidbody rb;
    private Animator anim;

    private Transform origin;
    private Transform destination;

    public float speed = 20f;
    public float rotationSpeed = 200f;

    private float lookTime = 0f;
    [SerializeField]
    private float moveTime = 3f;
    private float lookAwayTime = 0f;
    [SerializeField]
    private float moveBackTime = 3f;

    public bool moveWithNavMesh = false;
    public NavMeshAgent navMeshAgent;

    [SerializeField]
    private float stopBeforePlayer = 15f; 

    public Looker looker;

    private bool move = false;
    private float startTime;

    private bool movingToOrigin = false;
    private bool reachingToCube = false;

    public bool stoppable = true;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        steamVR_GazeTracker = GetComponent<SteamVR_GazeTracker>();
        anim = GetComponent<Animator>();
        origin = transform;
        steamVR_GazeTracker.GazeOn += Approach;
    }

    // Update is called once per frame
    void Update()
    {
        if (steamVR_GazeTracker.isInGaze && !move)
        {
            lookTime += Time.deltaTime;
            lookAwayTime = 0f;
        }  
        else
        {
            lookAwayTime += Time.deltaTime;
            lookTime = 0f;
        }    

        if (destination != null)
        {
            Debug.Log(destination.name);
            float distance = Vector3.Distance(transform.position, destination.position);

            if (stopBeforePlayer <= distance)
            {
                navMeshAgent.destination = destination.position;
                navMeshAgent.stoppingDistance = stopBeforePlayer;
            }
            else
            {

                if (!isMoving())
                {
                    DeactivateAnim();
                    destination = null;
                }
                else
                    ActivateAnim();
            }
        }

        if (lookAwayTime >= moveBackTime && stoppable)
        {
            MoveToOrigin();
        }

        WalkAnimCheck();
    }

    void Approach(object sender, GazeEventArgs gazeEventArgs)
    {
        var gazedByPosition = gazeEventArgs.hmd.transform.position;
        var directionVector = gazedByPosition - transform.position;
        if (stoppable && moveWithNavMesh && lookTime >= moveTime)
        {
             anim.SetBool("Walk_Anim", true);
             if (destination == null)
                 destination = gazeEventArgs.hmd.transform;
        }
    }

    public void MoveToOrigin()
    {
        destination = origin;
    }

    public void MoveTowards(Transform dest)
    {
        destination = dest;
    }



    public bool isMoving()
    {
        if (!navMeshAgent.pathPending)
            if (navMeshAgent.remainingDistance <= stopBeforePlayer)
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    return false;
                else
                    return true;
            else
                return true;
        else
            return false;
    }

    public void ReachedDestination()
    {
        stoppable = true;
        looker.ReachedDestination();
    }
    
    public void WalkAnimCheck()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= stopBeforePlayer)
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    DeactivateAnim();
        }
        else
        {
            ActivateAnim();
        }
             
    }
    
    public void ActivateAnim()
    {
        anim.SetBool("Walk_Anim", true);
    }

    public void DeactivateAnim()
    {
        anim.SetBool("Walk_Anim", false);
    }
}