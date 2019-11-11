using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looker : MonoBehaviour
{

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
        else
        {
            if (!coroutineRunning)
                DisableUI();
        }
    }

    public void ReachedDestination()
    {
        StartCoroutine(ArrivalDialogue());
    }
    
    public void ShowRedCube()
    {
        StartCoroutine(FollowMeStatement(redCube));
    }

    public void ShowBlueCube()
    {
        StartCoroutine(FollowMeStatement(blueCube));
    }

    public void DisableUI()
    {
        responsePanel.gameObject.SetActive(false);
        actionPanel.gameObject.SetActive(false);
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

    IEnumerator FollowMeStatement(GameObject destinationObject)
    {
        actionPanel.gameObject.SetActive(false);
        responsePanel.gameObject.SetActive(true);
        responseText.text = "Follow me";
        roboBehaviour.MoveTowards(destinationObject.transform);
        yield return new WaitForSeconds(3f);
        responsePanel.gameObject.SetActive(false);
        yield return null;
    }

}
