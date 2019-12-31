﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Valve.Newtonsoft.Json;
using Valve.VR;
using Valve.VR.Extras;

public class RoboBehaviour : MonoBehaviour
{
    
    /*
     * Controller:
     * - Idle
     * - Checks for:
     *     - in Range?
     *         - if true: Eye contact?
     *             - if true: Emotion right?
     *                 - if true: move towards
     *                 - trigger interaction
     *                     - if positive: move towards search object
     *                         - if object found: - check emotion
     *                             - if happy: - end interaction, return to start point
     *                             - if unhappy: - feedback interaction (Do you need more help?)
     *                                 - if true: - restart interaction at point x
     *                                 - if false: - end interaction, return to start point
     *                     - if negative: return to start position
     *                         - remember to not approach this player for x seconds
     *                 - if cant be read: - start interaction
     *     - if any is false: - dont move
     */
    
    private SteamVR_GazeTracker steamVR_GazeTracker;
    private Rigidbody rb;
    private Animator anim;

    public float speed = 20f;
    public float sonarRange = 5;

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
    public float stopBefore = 3f;
    public bool stoppable = true;
    private GameObject gazedBy;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        steamVR_GazeTracker = GetComponent<SteamVR_GazeTracker>();
        anim = GetComponent<Animator>();
        steamVR_GazeTracker.GazeOn += SetGazedBy;
    }

    public GameObject GetGazedBy()
    {
        return gazedBy;
    }

    void SetGazedBy(object sender, GazeEventArgs gazeEventArgs)
    {
        Vector3 gazedByPosition = gazeEventArgs.hmd.transform.position;
        gazedBy = gazeEventArgs.hmd.gameObject;
        
        
            /*
                StartCoroutine(MoveTo(gazedByPosition - (directionVector - (directionVector.normalized * stopBefore)),
                    -steamVR_GazeTracker.hmd.transform.forward));
          */
    }

    public void MoveTowards(Transform destination)
    {
        if (!moveWithNavMesh)
        {
            StartCoroutine(MoveTo(destination.position, -steamVR_GazeTracker.hmd.transform.forward, true));
        }
        else
        {
            navMeshAgent.SetDestination(destination.position);
        }
    }
    
    public List<GameObject> InRange(String tagToCheck)
    {
        RaycastHit[] raycastHits;
        raycastHits = Physics.SphereCastAll(transform.position, sonarRange, transform.forward);
        List<GameObject> validHits = new List<GameObject>();
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.transform.parent != null && hit.transform.parent.CompareTag(tagToCheck))
                validHits.Add(hit.transform.gameObject);
        }
        return validHits;
    }
    
    public bool NotHappy(EmotionState emotionState)
    {
        return true;
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