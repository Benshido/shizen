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

    bool attacking = false;
    public bool Attacking { get { return attacking; } }

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
        attacking = false;
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
                attacking = true;
                cancelReset = true;
                if (canGoToNextCombo)
                {
                    canGoToNextCombo = false;
                    comboCount++;
                }
            }
        }
    }

    public void EndOfComboReset()
    {
        comboCount = 0;
    }

    public IEnumerator ResetCombo(float seconds)
    {
        cancelReset = false;
        yield return new WaitForSecondsRealtime(seconds);
        if(!cancelReset) comboCount = 0;
    }

    public void NextComboAvailable()
    {
        canGoToNextCombo = true;
    }
}
