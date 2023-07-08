using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInit : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioManager.StartFade(audioMixer, "MasterVolume", 5.0f, 100f));
        AudioManager.globalEmitter = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
