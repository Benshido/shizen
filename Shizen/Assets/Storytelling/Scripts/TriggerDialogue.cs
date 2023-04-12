using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public GameObject dialogueManager;
    public int dialogueIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueManager.GetComponent<DialogueManager>().DialogueHasBeenTriggered(dialogueIndex);
        }
    }
}
