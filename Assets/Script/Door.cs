using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    float openDistance = 10f;

    GameObject[] players;
    GameObject[] helpers;
    GameObject[] doors;
    List<GameObject> objects;

    float distanceToDoor;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null)
            Debug.Log("No Players found");
        helpers = GameObject.FindGameObjectsWithTag("helper");
        if (helpers == null)
            Debug.Log("No Helpers found");
        objects = new List<GameObject>(players);
        objects.AddRange(helpers);
        doors = GameObject.FindGameObjectsWithTag("Door");
        if (doors == null)
            Debug.Log("No Doors found");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            distanceToDoor = 1000;
            for (int j = 0; j < objects.Count; j++)
            {
                float distance = Vector3.Distance(doors[i].transform.position, objects[j].transform.position);
                if (distanceToDoor != null)
                {
                    if (distance < distanceToDoor)
                        distanceToDoor = distance;
                }
                else
                {
                    distanceToDoor = distance;
                }
            }
            if (distanceToDoor < openDistance)
            {
                doors[i].SetActive(false);
            }
            else
            {
                doors[i].SetActive(true);
            }
        }
    }
}
