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

    private bool attacking = false;
    private bool isHeavy = false;
    private bool lastAtkWasHeavy = false;
    public bool Attacking { get { return attacking; } }
    public bool IsHeavy { get { return isHeavy; } }

    private List<string> elementList = Enum.GetNames(typeof(Element)).ToList();

    [SerializeField] KeyCode NextElement = KeyCode.E;
    [SerializeField] KeyCode PrevElement = KeyCode.Q;
    [SerializeField] KeyCode NormalAttack = KeyCode.Mouse0;
    [SerializeField] KeyCode HeavyAttack = KeyCode.Mouse1;

    private PlayerMovement playerMovement;
    private bool cancelReset = false;
    private float resetComboTime = 1;
    private float resetTimer = 0;
    private bool runTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cancelReset) { resetTimer = 0; runTimer = false; }

        if (runTimer)
        {
            resetTimer += Time.unscaledDeltaTime;
            if (resetTimer >= resetComboTime)
            {
                runTimer = false;
                resetTimer = 0;
                EndOfComboReset();
            }
        }

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
                //cancelReset = true;
                if (canGoToNextCombo && RequireStamina())
                {
                    if (lastAtkWasHeavy) comboCount = 0;
                    canGoToNextCombo = false;
                    lastAtkWasHeavy = false;
                    comboCount++;
                    attacking = true;
                    isHeavy = false;
                    StartCoroutine(CancelReset());
                }
            }
            if (Input.GetKeyDown(HeavyAttack) && playerMovement.IsGrounded)
            {
                if (canGoToNextCombo && RequireStamina())
                {
                    if (!lastAtkWasHeavy) comboCount = 0;
                    canGoToNextCombo = false;
                    lastAtkWasHeavy = true;
                    comboCount++;
                    attacking = true;
                    isHeavy = true;
                    StartCoroutine(CancelReset());
                }
            }
        }
    }

    public void EndOfComboReset()
    {
        comboCount = 0;
        NextComboAvailable();
    }

    public void ResetCombo(float seconds)
    {
        runTimer = true;
        resetComboTime = seconds;
    }

    private IEnumerator CancelReset()
    {
        var combocountNow = ComboCount;
        yield return new WaitUntil(() => comboCount != combocountNow && comboCount != 0);
        cancelReset = true;
    }

    public void NextComboAvailable()
    {
        canGoToNextCombo = true;
        attacking = false;
        isHeavy = false;
        cancelReset = false;
    }

    private bool RequireStamina()
    {
        float stamina = StaminaRequired();
        
        if (playerMovement.HP.EP >= stamina) return true;
        return false;
    }

    private float StaminaRequired()
    {
        if (isHeavy)
            switch (elementIndex)
            {
                case (int)Element.Earth:
                    return 8;
                default:
                    break;
            }
        else
        {
            switch (elementIndex)
            {
                case (int)Element.Earth:
                    return 2;
                default:
                    break;
            }
        }
        return 0;
    }
}
