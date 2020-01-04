using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalVars : MonoBehaviour
{
    public Canvas interactionCanvas;
    public GameObject helpPanel;
    public GameObject feedbackPanel;
    public GameObject searchingObjPanel;

    public float waitingForNextPanel = 2f;

    public Transform destination;

    public bool asked = false;
    public bool feedback = false;
}
