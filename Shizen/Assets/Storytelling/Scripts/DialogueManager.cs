using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue[] dialogue;
    public AudioSource audioSource;
    public GameObject dialogueUI;
    public GameObject dialogueInText;

    private void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void DialogueHasBeenTriggered(int dialogueIndex)
    {
        Debug.Log(dialogueIndex);
        if (!dialogue[dialogueIndex].hasBeenTriggered)
        {
            dialogueUI.SetActive(true);
            dialogueInText.GetComponent<TextMeshProUGUI>().text = dialogue[dialogueIndex].dialogueText;
            audioSource.PlayOneShot(dialogue[dialogueIndex].dialogueClip);
            dialogue[dialogueIndex].hasBeenTriggered = true;
            StartCoroutine(ShowDialogueBox(5));
        }
    }

    IEnumerator ShowDialogueBox(float time)
    {
        yield return new WaitForSeconds(time);
        dialogueUI.SetActive(false);
        StopAllCoroutines();
    }
}
