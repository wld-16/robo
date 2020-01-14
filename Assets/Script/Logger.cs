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
    public bool willDebug;

    void Start()
    {
        StreamWriter writer = new StreamWriter(logLocation, false);
        writer.WriteLine("timestamp;person position;;;person rotation;;;robot position;;;robot rotation;emotions");
        writer.WriteLine(";x;y;z;x;y;z;x;y;z;x;y;z;Angry;Sad;Happy;Disgust;Surprise;Fear;Neutral");
        writer.Close();
    }
    
    void Update()
    {
        string printFileFrame = "" + DateTime.Now + ";";
        string frame = "";
        
        Vector3 playerPosition = playerHead.position;
        Vector3 playerRotation = playerHead.rotation.eulerAngles;
        Vector3 robotHeadPosition = robotHead.position;
        Vector3 robotHeadRotation = robotHead.rotation.eulerAngles;

        frame += string.Format("[person position/x={0},y={1},z={2}]", playerPosition.x, playerPosition.y, playerPosition.z);
        frame += string.Format("[person rotation/x={0},y={1},z={2}]", playerRotation.x, playerRotation.y, playerRotation.z);
        frame += string.Format("[robot position/x={0},y={1},z={2}]", robotHeadPosition.x, robotHeadPosition.y, robotHeadPosition.z);
        frame += string.Format("[robot rotation/x={0},y={1},z={2}]", robotHeadRotation.x, robotHeadRotation.y, robotHeadRotation.z);

        
        printFileFrame += string.Format("{0};{1};{2};", playerPosition.x, playerPosition.y, playerPosition.z);
        printFileFrame += string.Format("{0};{1};{2};", playerRotation.x, playerRotation.y, playerRotation.z);
        printFileFrame += string.Format("{0};{1};{2};", robotHeadPosition.x, robotHeadPosition.y, robotHeadPosition.z);
        printFileFrame += string.Format("{0};{1};{2};", robotHeadRotation.x, robotHeadRotation.y, robotHeadRotation.z);

        
        if (seeEmotion != null && seeEmotion.emotions != null)
        {
            string emotionsString = "";
            seeEmotion.emotions.ForEach(pair => emotionsString += pair.Value + "; ");
            printFileFrame += emotionsString;
            
            string emotionsDebugString = "[";
            seeEmotion.emotions.ForEach(pair => emotionsDebugString += pair.Key + "=" + pair.Value + ", ");
            emotionsDebugString = emotionsDebugString.Substring(0, emotionsDebugString.Length - 2) + "]";

            frame += emotionsDebugString;
        }

        if (willDebug)
        {
            Debug.Log(frame);
        }
        WriteString(printFileFrame, logLocation);
        
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
