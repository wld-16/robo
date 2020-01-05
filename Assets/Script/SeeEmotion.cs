using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SeeEmotion : MonoBehaviour
{
    public float IntensityFactor = 1.0f;

    public Dictionary<Emotion, float> emotions;
    public bool simulate;
    [Range(0,1)]
    public float happySimulation = 1;
    [Range(0,1)]
    public float angrySimulation = 0;
    [Range(0,1)]
    public float sadSimulation = 0;
    [Range(0,1)]
    public float disgustSimulation = 0;
    [Range(0,1)]
    public float surpriseSimulation = 0;
    [Range(0,1)]
    public float fearSimulation = 0;
    [Range(0,1)]
    public float neutralSimulation = 0;


    protected Light __light
    {
        get { return this.GetComponent<Light>(); }
    }

    protected Emotion? __currentEmotion = null;

    void Start()
    {
        emotions = new Dictionary<Emotion, float>();
        emotions.Add(Emotion.Angry, 0);
        emotions.Add(Emotion.Sad, 0);
        emotions.Add(Emotion.Happy, 0);
        emotions.Add(Emotion.Disgust, 0);
        emotions.Add(Emotion.Surprise, 0);
        emotions.Add(Emotion.Fear, 0);
        emotions.Add(Emotion.Neutral, 0);
    }
    
    private void OnEnable()
    {
        GameObject gcO = GameObject.FindWithTag("GameController");
        if (gcO && !simulate)
        {
            WebCamEmotionClient wcec = gcO.GetComponent<WebCamEmotionClient>();
            if (wcec)
            {
                wcec.OnChange += this.OnWebCam;
            }
            else
            {
                this.enabled = false;
            }
        } 
    }

    private void OnDisable()
    {
        GameObject gcO = GameObject.FindWithTag("GameController");
        if (gcO)
        {
            WebCamEmotionClient wcec = gcO.GetComponent<WebCamEmotionClient>();
            if (wcec)
            {
                wcec.OnChange -= this.OnWebCam;
            }
        } 
    }

    private void Update()
    {
        if (simulate)
        {
            emotions[Emotion.Angry] = angrySimulation;
            emotions[Emotion.Sad] = sadSimulation;
            emotions[Emotion.Happy] = happySimulation;
            emotions[Emotion.Disgust] = disgustSimulation;
            emotions[Emotion.Surprise] = surpriseSimulation;
            emotions[Emotion.Fear] = fearSimulation;
            emotions[Emotion.Neutral] = neutralSimulation;
            
        }
    }

    protected void OnWebCam(EmotionState[] newStates, EmotionState[] oldStates)
    {
        if (
            oldStates == null
            || this.__currentEmotion == null
            || ((EmotionState) oldStates[0]).GetValue((Emotion) this.__currentEmotion) * 0.9 >
            newStates[0].GetValue((Emotion) this.__currentEmotion)
        )
        {
            Emotion highestEmotion = newStates[0].GetHighestEmotion(new[] {Emotion.Neutral});
            float emotionValue = newStates[0].GetValue(highestEmotion);
            EmotionState state = newStates[0];
            
            emotions[Emotion.Angry] =  state.Angry;
            emotions[Emotion.Sad] =  state.Sad;
            emotions[Emotion.Happy] =  state.Happy;
            emotions[Emotion.Disgust] =  state.Disgust;
            emotions[Emotion.Surprise] =  state.Surprise;
            emotions[Emotion.Fear] =  state.Fear;
            emotions[Emotion.Neutral] =  state.Neutral;

            Debug.Log(String.Format(
                "[Angry={0}, Sad={1}, Happy={2}, Disgust={3}, Surprise={4}, Fear={5}, Neutral={6}", state.Angry,
                state.Sad, state.Happy, state.Disgust, state.Surprise, state.Fear, state.Neutral));
        }
    }
}