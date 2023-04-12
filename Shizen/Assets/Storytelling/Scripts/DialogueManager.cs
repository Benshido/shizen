using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue[] dialogue;
    public GameObject[] logs;
    public AudioSource audioSource;
    public GameObject dialogueUI;
    public GameObject dialogueInText;

    private void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void DialogueHasBeenTriggered(int dialogueIndex)
    {
        if (!dialogue[dialogueIndex].hasBeenTriggered)
        {
            audioSource.Stop();
            dialogueUI.SetActive(false);
            dialogueUI.SetActive(true);
            dialogueInText.GetComponent<TextMeshProUGUI>().text = dialogue[dialogueIndex].dialogueText;
            logs[dialogueIndex].GetComponent<TextMeshProUGUI>().text = dialogue[dialogueIndex].dialogueText;
            audioSource.PlayOneShot(dialogue[dialogueIndex].dialogueClip);
            dialogue[dialogueIndex].hasBeenTriggered = true;
            StartCoroutine(ShowDialogueBox(5));
        }
    }

    public void PlayAudio(int dialogueIndex)
    {
        if (dialogue[dialogueIndex].hasBeenTriggered)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(dialogue[dialogueIndex].dialogueClip);
        }
    }

    IEnumerator ShowDialogueBox(float time)
    {
        yield return new WaitForSeconds(time);
        dialogueUI.SetActive(false);
        StopAllCoroutines();
    }
}
