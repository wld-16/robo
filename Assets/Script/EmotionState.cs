using System;
using System.Linq;
using System.Runtime.InteropServices;

public enum Emotion : byte {
    Angry = 0,
    Disgust = 1,
    Fear = 2,
    Happy = 3,
    Sad = 4,
    Surprise = 5,
    Neutral = 6
}

[StructLayout (LayoutKind.Sequential, Pack = 1)]
public struct EmotionState {
    float Angry;
    float Disgust;
    float Fear;
    float Happy;
    float Sad;
    float Surprise;
    float Neutral;

    public Emotion GetHighestEmotion (Emotion[] ignore = null) {
        byte[] ignoreBytes = null;

        if (ignore != null) {
            ignoreBytes = new byte[ignore.Length];

            for (byte i = 0; i < ignore.Length; i++) {
                ignoreBytes [i] = (byte) ignore [i];
            }
        }

        return GetHighestEmotion (this.ToArray (), ignoreBytes);
    }

    public static Emotion GetHighestEmotion (float[] emotionState, byte[] ignore = null) {
        float highestValue = Single.NegativeInfinity;
        byte index = Byte.MaxValue;

        for (byte i = 0; i < emotionState.Length; i++) {
            if (emotionState [i] > highestValue && (ignore == null || ignore.Contains (i) == false)) {
                index = i;
                highestValue = emotionState [i];
            }
        }

        return (Emotion) index;
    }

    public float GetValue (Emotion emotion) {
        return GetValue (this.ToArray (), emotion);
    }

    public static float GetValue (float[] emotionState, Emotion emotion) {
        return emotionState [(byte) emotion];
    }

    public float[] ToArray () {
        return new[] {
            this.Angry,
            this.Disgust,
            this.Fear,
            this.Happy,
            this.Sad,
            this.Surprise,
            this.Neutral
        };
    }

    public static EmotionState FromArray (float[] emotionState) {
        return new EmotionState () {
            Angry = emotionState [0],
            Disgust = emotionState [1],
            Fear = emotionState [2],
            Happy = emotionState [3],
            Sad = emotionState [4],
            Surprise = emotionState [5],
            Neutral = emotionState [6]
        };
    }
}