using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformPuzzle : MonoBehaviour
{
    public GameObject platform;
    public GameObject player;
    public GameObject text;
    public AnimationClip clip;
    bool platformTriggered = false;

    private void Start()
    {
        text.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Outline>().enabled = false;
            if (!platformTriggered) text.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.R) && !platformTriggered)
            {
                platform.GetComponent<Animator>().SetTrigger("PlatformTriggered");
                text.SetActive(false);
                platformTriggered = true;
                player.GetComponent<Animator>().Play(clip.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.SetActive(false);
        }
    }
}
