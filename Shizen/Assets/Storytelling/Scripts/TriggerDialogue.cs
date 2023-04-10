using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            audioSource.PlayOneShot(clip);
        }
    }
}
