using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    public static AudioInit globalEmitter;
    public static AudioListener listener;
    public static List<AudioSource> sources = new List<AudioSource>();

    public static void PlaySound(string eventName, int pitchFactor = 0, GameObject emitter = null, bool randomPitch = false)
    {
        //播的时候触发一次清理
        if (sources.Count > 0)
        {
            for (int i = sources.Count - 1; i > 0; i--)
            {
                if (sources[i] == null)
                {
                    sources.RemoveAt(i);
                    continue;
                }
                if (!sources[i].isPlaying)
                {
                    GameObject.Destroy(sources[i]);
                    sources.RemoveAt(i);
                }
            }
        }
        if (emitter == null)
        {
            emitter = globalEmitter.gameObject;
            AudioSource source = emitter.AddComponent<AudioSource>();
            sources.Add(source);
            var clip = Resources.Load<AudioClip>("Audio/SFX/" + eventName);
            if (clip != null)
                source.clip = clip;
            if (pitchFactor > 0)
            {
                source.pitch = 1 + (pitchFactor - 1) / 10f;
            }
            source.Play();
        }
        else
        {
            AudioSource source = emitter.AddComponent<AudioSource>();
            sources.Add(source);
            var clip = Resources.Load<AudioClip>("Audio/SFX/" + eventName);
            if (clip != null)
                source.clip = clip;
            if (pitchFactor > 0)
            {
                source.pitch = 1 + (pitchFactor - 1) / 10f;
            }
            source.volume = Mathf.Lerp(1, 0, Vector3.Distance(source.gameObject.transform.position, listener.transform.position) / 50);
            if (randomPitch)
            {
                float pitch = Random.Range(-0.5f, 0.5f);
                source.pitch += pitch;
            }
            source.Play();
        }
    }

    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
