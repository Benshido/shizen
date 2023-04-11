using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceDestroyer : MonoBehaviour
{
    private AudioSource source;

    public void Destroyer()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(DestroyWhenStoppedPlaying());
    }

    private IEnumerator DestroyWhenStoppedPlaying()
    {
        yield return new WaitUntil(() => !source.isPlaying);
        Destroy(gameObject);
    }
}
