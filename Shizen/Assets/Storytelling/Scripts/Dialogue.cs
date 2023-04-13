using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [SerializeField] public string dialogueText;
    [SerializeField] public AudioClip dialogueClip;
    [SerializeField] public bool hasBeenTriggered;
}
