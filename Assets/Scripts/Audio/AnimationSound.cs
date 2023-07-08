using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    public AudioSource AudioSource;
    public void playAnimationSound(string eventName)
    {
        int suffix = UnityEngine.Random.Range(1, 5);
        var clip = Resources.Load<AudioClip>("Audio/SFX/Avatar/" + eventName + "_0" + suffix.ToString());
        if (clip != null)
            AudioSource.clip = clip;
        AudioSource.Play();
    }
}
