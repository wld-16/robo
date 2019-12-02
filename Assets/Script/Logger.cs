using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Valve.VR.InteractionSystem;

public class Logger : MonoBehaviour
{
    public SeeEmotion seeEmotion;
    public Transform playerHead;
    public Transform robotHead;

    void Update()
    {
        string frame = "[" + DateTime.Now + "]";

        Vector3 playerPosition = playerHead.position;
        Vector3 playerRotation = playerHead.rotation.eulerAngles;
        Vector3 robotHeadPosition = robotHead.position;
        Vector3 robotHeadRotation = robotHead.rotation.eulerAngles;

        frame += string.Format("[person position/x={0},y={1},z={2}]", playerPosition.x, playerPosition.y, playerPosition.z);
        frame += string.Format("[person rotation/x={0},y={1},z={2}]", playerRotation.x, playerRotation.y, playerRotation.z);
        frame += string.Format("[robot position/x={0},y={1},z={2}]", robotHeadPosition.x, robotHeadPosition.y, robotHeadPosition.z);
        frame += string.Format("[robot rotation/x={0},y={1},z={2}]", robotHeadRotation.x, robotHeadRotation.y, robotHeadRotation.z);

        if (seeEmotion != null && seeEmotion.emotions != null)
        {
            string emotionsString = "[";
            seeEmotion.emotions.ForEach(pair => emotionsString += pair.Key + "=" + pair.Value + ", ");
            emotionsString = emotionsString.Substring(0, emotionsString.Length - 2) + "]";

            frame += emotionsString;
        }
        
        Debug.Log(frame);
    }
}