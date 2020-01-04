using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalVars : MonoBehaviour
{
    public Canvas interactionCanvas;
    public GameObject helpPanel;
    public GameObject feedbackPanel;
    public GameObject searchingObjPanel;

    public Vector3 roboOrigin;

    public float waitingForNextPanel = 2f;

    public Vector3 destination;

    public bool asked = false;
    public bool feedback = false;
}
