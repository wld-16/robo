using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("helper"))
        {
            if (other.GetComponent<RoboBehaviour>().moveWithNavMesh)
                Debug.Log("Goal");
                //other.GetComponent<RoboBehaviour>().ReachedDestination();
        } 
    }
}
