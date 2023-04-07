using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformPuzzle : MonoBehaviour
{
    public GameObject platform;
    public GameObject player;
    public AnimationClip clip;
    bool platformTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //SHOW THAT THE PLAYER SHOULD PRESS E ON CANVAS
        Debug.Log("EnterTrigger!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && !platformTriggered)
        {
            Debug.Log("CLICKED!");
            platform.GetComponent<Animator>().SetTrigger("PlatformTriggered");
            platformTriggered = true;
            player.GetComponent<Animator>().Play(clip.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //HIDE THAT THE PLAYER SHOULD PRESS E ON CANVAS.
        Debug.Log("EXITTrigger!");
    }
}
