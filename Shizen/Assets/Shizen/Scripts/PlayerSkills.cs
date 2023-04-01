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
    public bool Attacking { get { return attacking; } }

    private List<string> elementList = Enum.GetNames(typeof(Element)).ToList();

    [SerializeField] KeyCode NextElement = KeyCode.E;
    [SerializeField] KeyCode PrevElement = KeyCode.Q;
    [SerializeField] KeyCode NormalAttack = KeyCode.Mouse0;
    [SerializeField] KeyCode HeavyAttack = KeyCode.Mouse2;

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
                Debug.Log("reset");
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
                if (canGoToNextCombo)
                {
                    canGoToNextCombo = false;
                    comboCount++;
                    attacking = true;
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

    int resetsRunning = 0;
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
        cancelReset = false;
    }
}
