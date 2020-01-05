using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Valve.VR.InteractionSystem;

public class Logger : MonoBehaviour
{
    public SeeEmotion seeEmotion;
    public Transform playerHead;
    public Transform robotHead;
    private HandleTextFile handleTextFile;
    public string logLocation;

    void Start()
    {
        StreamWriter writer = new StreamWriter(logLocation, false);
        writer.WriteLine("");
        writer.Close();
    }
    
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
        
        WriteString(frame, logLocation);
        
    }
    [MenuItem("Tools/Write file")]
    static void WriteString(string content, string path)
    {

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(content);
        writer.Close();
        
    }

    [MenuItem("Tools/Read file")]
    static void ReadString(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
