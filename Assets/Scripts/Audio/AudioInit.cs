using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioInit : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioManager.StartFade(audioMixer, "MasterVolume", 5.0f, 100f));
        AudioManager.globalEmitter = this;
        AudioManager.listener = FindFirstObjectByType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(string eventName)
    {
        StartCoroutine(AudioManager.FadeOut(audioSource, 0.5f));
        var clip = Resources.Load<AudioClip>("Audio/BGM/" + eventName);
        if (clip != null)
            audioSource.clip = clip;
        audioSource.Play();
    }
}
