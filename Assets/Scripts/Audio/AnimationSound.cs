using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isLooping;
    public void playAnimationSound(AnimationEvent soundEvent)
    {
        var random = soundEvent.intParameter;
        var loop = soundEvent.floatParameter;
        var eventName = soundEvent.stringParameter;
        if (eventName == "StopLoop")
        {
            isLooping = false;
            audioSource.loop = false;
            StartCoroutine(AudioManager.FadeOut(audioSource, 0.5f));
            return;
        }
        if (random != 0)
        {
            int suffix = UnityEngine.Random.Range(1, random + 1);
            DoPlayAudio(eventName + "_0" + suffix.ToString());
        }
        else
        {
            if (loop == 1)
            {
                if (!isLooping) // Play a loop sfx for the first time
                {
                    audioSource.loop = true;
                    isLooping = true;
                    DoPlayAudio(eventName);
                }
            }
            else
            {
                DoPlayAudio(eventName);
            }
        }
    }

    private void DoPlayAudio(string eventName)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        var clip = Resources.Load<AudioClip>("Audio/SFX/" + eventName);
        Debug.Log(clip);
        if (clip != null)
            audioSource.clip = clip;
        audioSource.Play();
    }
}
