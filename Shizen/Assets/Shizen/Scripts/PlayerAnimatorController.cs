using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerSkills playerSkills;
    [Tooltip("If null will grab from this object")]
    [SerializeField] Animator animator;
    [SerializeField] Transform modelTransform;
    [SerializeField] ParticleSystem dashParticles;
    private List<List<Animator>> elementAnimators = new();

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetTrigger("Idle");

        for (int i = 0; i < Unlockables.Elements.Count; i++)
        {
            elementAnimators.Add(new());
        }
    }

    void Update()
    {
        //Debug.Log(playerSkills.ComboCount);
        if (playerMovement.HP.IsAlive)
        {
            animator.SetBool("BackStep", playerMovement.BackStep);
            animator.SetInteger("AttackType", playerSkills.ElementIndex);
            animator.SetInteger("ComboCount", playerSkills.ComboCount);
            animator.SetBool("Attacking", playerSkills.Attacking);

            if (playerMovement.IsDashing)
            {
                playerMovement.StopJump();
                playerSkills.EndOfComboReset();
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

                animator.SetBool("Jump", playerMovement.IsJumping);
                animator.SetBool("Falling", !playerMovement.IsGrounded);
            }
        }
        else
        {
            playerMovement.CanMove(0);
            //animator.SetTrigger("Dead");
        }
    }

    //All below methods can be called from animation events

    public void ResetCombo(float seconds)
    {
        StartCoroutine(playerSkills.ResetCombo(seconds));
    }

    public void SpawnPrefab(GameObject prefab)
    {
        if (Unlockables.Elements[Enum.GetName(typeof(Element), playerSkills.ElementIndex)] > 0)
        {
            var pref = Instantiate(prefab, modelTransform);
            pref.transform.parent = null;
            elementAnimators[playerSkills.ElementIndex].Add(pref.GetComponentInChildren<Animator>());
        }
    }

    public void UpdateElementComboIndex(int index)
    {

        var elem = elementAnimators[playerSkills.ElementIndex];

        //uses only latest spawned elemental object of said element
        if (elem.Count > 0 && elem != null) elem[elementAnimators[playerSkills.ElementIndex].Count - 1].SetInteger("ComboStage", index);

    }

    public void RemoveFromList(Element elem, Animator anim)
    {
        if (elementAnimators[(int)elem].IndexOf(anim) == elementAnimators[(int)elem].Count - 1)
            playerSkills.EndOfComboReset();
        elementAnimators[(int)elem].Remove(anim);
    }

    public void DashParticlePlay()
    {
        if (!dashParticles.isPlaying) dashParticles.Play();
    }
}
