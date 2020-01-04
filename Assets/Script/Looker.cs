using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looker : MonoBehaviour
{
    public globalVars gV;
    public Transform[] destinations;

    public Dropdown dropdown;

    public GameObject lookAt;
    public GameObject responsePanel;
    public Text responseText;
    public GameObject actionPanel;
    public float rangeShow = 40f;
    public bool coroutineRunning = false;
    public RoboBehaviour roboBehaviour;
    
    public Transform origin;

    public GameObject blueCube;
    public GameObject redCube;
    
    // Start is called before the first frame update
    void Start()
    {
        responseText = responsePanel.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, lookAt.transform.position) <= rangeShow)
        {
            if (!coroutineRunning)
            {
                coroutineRunning = true;
                StartCoroutine(HelpDialog());
            } 
            transform.rotation = Quaternion.LookRotation(transform.position - lookAt.transform.position);
        }
    }

    public void SetDestination()
    {
        Debug.Log(dropdown.options[dropdown.value]);
        //gV.destination = destinations[option];
    }

    public void ReachedDestination()
    {
        StartCoroutine(ArrivalDialogue());
    }
    
    public void ShowRedCube()
    {
        StartCoroutine(FollowMeStatement());
    }

    public void ShowBlueCube()
    {
        StartCoroutine(FollowMeStatement());
    }
    
    IEnumerator HelpDialog()
    {
        responsePanel.gameObject.SetActive(true);
        responseText.text = "Need Help?";
        yield return new WaitForSeconds(3f);
        actionPanel.gameObject.SetActive(true);
        responsePanel.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator ArrivalDialogue()
    {
        responsePanel.gameObject.SetActive(true);
        Debug.Log("arrived");
        responseText.text = "There you go!";
        yield return new WaitForSeconds(5f);
        roboBehaviour.MoveTowards(origin);
        responseText.text = "I go back to my origin now!";
        coroutineRunning = false;
        yield return null;
    }

    IEnumerator FollowMeStatement()
    {
        actionPanel.gameObject.SetActive(false);
        responsePanel.gameObject.SetActive(true);
        responseText.text = "Follow me";
        roboBehaviour.MoveTowards(blueCube.transform);
        yield return new WaitForSeconds(3f);
        responsePanel.gameObject.SetActive(false);
        yield return null;
    }

}
