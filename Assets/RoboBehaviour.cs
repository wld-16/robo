using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class RoboBehaviour : MonoBehaviour
{
    private SteamVR_GazeTracker steamVR_GazeTracker;
    private Rigidbody rb;
    private Animator anim;

    public float speed = 2f;

    public float lookTime = 0f;
    public float moveTime = 3f;

    public float rotationStep = 200;

    private bool move = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        steamVR_GazeTracker = GetComponent<SteamVR_GazeTracker>();
        anim = GetComponent<Animator>();
        //steamVR_GazeTracker.GazeOn += Approach;
    }

    void Approach(object sender, GazeEventArgs gazeEventArgs)
    {
        Debug.Log("Approach");
        while(gazeEventArgs.distance > 5f)
        {
            rb.AddForce((gazeEventArgs.hmd.transform.position - transform.position).normalized * speed);
            Debug.DrawRay(transform.position, Vector3.Normalize(gazeEventArgs.hmd.transform.position - transform.position) * 1000f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (steamVR_GazeTracker.isInGaze)
            lookTime += Time.deltaTime;
        else
            lookTime = 0;

        if (lookTime >= moveTime)
            move = true;
        else
            move = false;

        if (move && Vector3.Distance(transform.position, steamVR_GazeTracker.hmd.transform.position) > 15f)
        {
            rb.AddForce((steamVR_GazeTracker.hmd.transform.position - transform.position).normalized * speed);
            anim.SetBool("Walk_Anim", true);
            rb.AddTorque(Vector3.Lerp(transform.localRotation.eulerAngles, -steamVR_GazeTracker.hmd.transform.forward, 1f));
            //transform.rotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("Walk_Anim", false);
        }   
    }
}
