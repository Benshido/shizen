using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public GameObject dialogueManager;
    public GameObject[] outlineHints;
    public int dialogueIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueManager.GetComponent<DialogueManager>().DialogueHasBeenTriggered(dialogueIndex);

            if (outlineHints != null)
            {
                for (int i = 0; i < outlineHints.Length; i++)
                {
                    outlineHints[i].GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
