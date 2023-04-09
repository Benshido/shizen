using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class ClipArray 
{
    public AudioClip[] clips;
    public AudioSource source;
    public AudioMixerGroup group;
    [Header("Loop")]
    [SerializeField] bool seamlessLoop;

    [Header("Pitch")]
    public bool randomPitch;
    public float minPitch = 0.75f;
    public float maxPitch = 1.5f;
    private float currentPitch = 1f;
    private AudioClip currentClip;


    public void PlayAudio()
    {
        SetPitch();
        SetClip();

        source.pitch = currentPitch;
        source.clip = currentClip;
        source.loop = seamlessLoop;
        source.Play();
    }
    public void SetClip()
    {
        if (currentClip == null) currentClip = clips[0];
        if (clips.Length > 1)
        {
            var prevClip = currentClip;
            var random = UnityEngine.Random.Range(1, clips.Length - 1);
            currentClip = clips[random];
            clips[0] = currentClip;
            clips[random] = prevClip;
        }
        else currentClip = clips[0];
    }
    public void SetPitch()
    {
        if (randomPitch) currentPitch = UnityEngine.Random.Range(minPitch, maxPitch);
        else currentPitch = 1;
    }
}

