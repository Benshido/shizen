using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerSkills playerSkills;
    [Tooltip("If null will grab from this object")]
    [SerializeField] Animator animator;
    private List<List<Animator>> elementAnimators = new();

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetTrigger("Idle");

        for (int i = 0; i < Unlockables.Elements.Count; i++)
        {
            elementAnimators.Add(new());
            Debug.Log(elementAnimators[i].Count);
        }
    }

    void Update()
    {
        if (playerMovement.HP.IsAlive)
        {
            animator.SetBool("BackStep", playerMovement.BackStep);
            animator.SetInteger("AttackType", playerSkills.ElementIndex);
            animator.SetInteger("ComboCount", playerSkills.ComboCount);
            animator.SetBool("Attack", playerSkills.Attacking);
            if (playerSkills.Attacking) CanMove(0);

            if (playerMovement.IsDashing)
            {
                StopJump();
                EndOfComboReset();
                animator.SetTrigger("Dash");
                animator.SetBool("Falling", false);
            }
            else
            {
                if (playerMovement.IsGrounded)
                {
                    if (playerMovement.IsRunning) { animator.SetTrigger("Move"); }
                    else if (!playerMovement.IsMoving) animator.SetTrigger("Idle");
                }

                if (playerMovement.IsJumping) animator.SetBool("Jump", true);
                animator.SetBool("Falling", !playerMovement.IsGrounded);
            }
        }
        else
        {
            playerMovement.CanMove(false);
            //animator.SetTrigger("Dead");
        }
    }

    //All below methods can be called from animation events
    public void StopJump()
    {
        animator.SetBool("Jump", false);
        playerMovement.StopJump();
    }

    public void Jump()
    {
        playerMovement.Jump();
    }

    public void CanMove(int value)
    {
        if (value == 0) playerMovement.CanMove(false);
        else playerMovement.CanMove(true);
    }

    public void ResetCombo(float seconds)
    {
        StartCoroutine(playerSkills.ResetCombo(seconds));
    }

    public void EndOfComboReset()
    {
        playerSkills.EndOfComboReset();
    }

    public void NextComboAvailable()
    {
        playerSkills.NextComboAvailable();
    }

    public void SpawnPrefab(GameObject prefab)
    {
        var pref = Instantiate(prefab, transform);
        pref.transform.parent = null;
        elementAnimators[playerSkills.ElementIndex].Add(pref.GetComponentInChildren<Animator>());
    }

    public void UpdateElementComboIndex(int index)
    {
        var elem = elementAnimators[playerSkills.ElementIndex];
        if (elem.Count > 0 && elem != null) elem[elementAnimators[playerSkills.ElementIndex].Count - 1].SetInteger("ComboStage", index);
    }
}
