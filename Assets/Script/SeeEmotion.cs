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

    protected Light __light
    {
        get { return this.GetComponent<Light>(); }
    }

    protected Emotion? __currentEmotion = null;

    private void OnEnable()
    {
        GameObject gcO = GameObject.FindWithTag("GameController");
        if (gcO)
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
            
            Dictionary<Emotion, float> emotionsTemp = new Dictionary<Emotion, float>();
            emotions.Add(Emotion.Angry, state.Angry);
            emotions.Add(Emotion.Sad, state.Sad);
            emotions.Add(Emotion.Happy, state.Happy);
            emotions.Add(Emotion.Disgust, state.Disgust);
            emotions.Add(Emotion.Surprise, state.Surprise);
            emotions.Add(Emotion.Fear, state.Fear);
            emotions.Add(Emotion.Neutral, state.Neutral);
            emotions = emotionsTemp;

            Debug.Log(String.Format(
                "[Angry={0}, Sad={1}, Happy={2}, Disgust={3}, Surprise={4}, Fear={5}, Neutral={6}", state.Angry,
                state.Sad, state.Happy, state.Disgust, state.Surprise, state.Fear, state.Neutral));
        }
    }
}