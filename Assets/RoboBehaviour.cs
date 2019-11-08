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

    public float speed = 20f;

    public float lookTime = 0f;
    public float moveTime = 3f;
    public bool moveWithNavMesh = false;
    public NavMeshAgent navMeshAgent;
    public float rotationSpeed = 200f;
    public Looker looker;

    private bool move = false;
    private float startTime;
    private bool movingToOrigin = false;
    private bool reachingToCube = false;
    private float stopBefore = 3f;
    public bool stoppable = true;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        steamVR_GazeTracker = GetComponent<SteamVR_GazeTracker>();
        anim = GetComponent<Animator>();
        steamVR_GazeTracker.GazeOn += Approach;
    }

    void Approach(object sender, GazeEventArgs gazeEventArgs)
    {
        var gazedByPosition = gazeEventArgs.hmd.transform.position;
        var directionVector = gazedByPosition - transform.position;

        if (stoppable)
        {
            if (moveWithNavMesh)
            {
                anim.SetBool("Walk_Anim", true);
                navMeshAgent.SetDestination(gazedByPosition -
                                            (directionVector.normalized * stopBefore));
            }
            else
            {
                StartCoroutine(MoveTo(gazedByPosition - (directionVector - (directionVector.normalized * stopBefore)),
                    -steamVR_GazeTracker.hmd.transform.forward));
            }   
        }
    }

    public void MoveTowards(Transform destination)
    {
        if (!moveWithNavMesh)
        {
            StartCoroutine(MoveTo(destination.position, -steamVR_GazeTracker.hmd.transform.forward, true));
        }
        else
        {
            if (destination.GetComponent<GoalTrigger>() != null) stoppable = false;
            navMeshAgent.SetDestination(destination.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (steamVR_GazeTracker.isInGaze && !move)
            lookTime += Time.deltaTime;
        else
            lookTime = 0;
    }

    IEnumerator MoveTo(Vector3 goalPosition, Vector3 rotationAngle, bool reachingForShowpoint = false)
    {
        move = true;
        rb.angularDrag = 0f;
        anim.SetBool("Walk_Anim", true);
        while (Vector3.Distance(transform.position, goalPosition) > stopBefore)
        {
            rb.AddForce((goalPosition - transform.position).normalized * speed);
            rb.AddTorque(Vector3.Lerp(transform.rotation.eulerAngles, rotationAngle,
                             lookTime / 3) * rotationSpeed);
            yield return null;
        }

        move = false;
        rb.angularDrag = Mathf.Infinity;
        rb.velocity = Vector3.zero;
        anim.SetBool("Walk_Anim", false);
        if (reachingForShowpoint) looker.ReachedDestination();
        yield return null;
    }

    public void ReachedDestination()
    {
        stoppable = true;
        looker.ReachedDestination();
    }
}