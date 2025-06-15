using System;
using UnityEngine;

[Serializable]
public class AudioSettings : SettingsBase
{
    public AudioSettings(float masterVolume, float music, float sfx)
    {
        MasterVolume = masterVolume;
        MusicVolume = music;
        SFXVolume = sfx;
    }

    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;

    public static float DecibelToLinear(float decibel)
    {
        return Mathf.Pow(10f, decibel / 20f);
    }
    public static float LinearToDecibel(float linear)
    {
        return 20f * Mathf.Log10(linear);
    }
}