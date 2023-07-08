using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    public static AudioInit globalEmitter;

    public static void PlaySound(string eventName, int pitchFactor = 0, bool randomPitch = false)
    {
        var ge = globalEmitter.audioSource;
        if (globalEmitter.audioSource.isPlaying)
        {
            globalEmitter.audioSource.Stop();
        }
        var clip = Resources.Load<AudioClip>("Audio/SFX/" + eventName);
        if (clip != null)
            globalEmitter.audioSource.clip = clip;
        if (pitchFactor > 0)
        {
            globalEmitter.audioSource.pitch = 1 + (pitchFactor - 1)/10f;
        }
        globalEmitter.audioSource.Play();

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
