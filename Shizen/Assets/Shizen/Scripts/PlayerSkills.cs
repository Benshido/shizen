using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] int elementIndex = 0;
    public int ElementIndex { get { return elementIndex; } }

    [SerializeField] int comboCount = 0;
    public int ComboCount { get { return comboCount; } }
    private bool canGoToNextCombo = true;

    private List<string> elementList = Enum.GetNames(typeof(Element)).ToList();

    [SerializeField] KeyCode NextElement = KeyCode.E;
    [SerializeField] KeyCode PrevElement = KeyCode.Q;
    [SerializeField] KeyCode NormalAttack = KeyCode.Mouse0;
    [SerializeField] KeyCode HeavyAttack = KeyCode.Mouse2;

    private PlayerMovement playerMovement;
    private bool cancelReset = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.HP.IsAlive)
        {
            if (Input.GetKeyDown(NextElement))
            {
                elementIndex++;
                if (elementIndex >= elementList.Count) elementIndex = 0;

            }
            if (Input.GetKeyDown(NextElement))
            {
                elementIndex--;
                if (elementIndex < 0) elementIndex = elementList.Count - 1;
            }
            if (Input.GetKeyDown(NormalAttack) && playerMovement.IsGrounded)
            {
                cancelReset = true;
                if (canGoToNextCombo)
                {
                    canGoToNextCombo = false;
                    comboCount++;
                    Debug.Log("AddedToCombo");
                }
            }
        }
    }

    public void EndOfComboReset()
    {
        comboCount = 0;
        Debug.Log("ResetCount");
    }

    public IEnumerator ResetCombo(float seconds)
    {
        cancelReset = false;
        yield return new WaitForSecondsRealtime(seconds);
        if(!cancelReset) comboCount = 0;
        Debug.Log("Count reset canceled = "+ cancelReset);
    }

    public void NextComboAvailable()
    {
        canGoToNextCombo = true;
    }
}
