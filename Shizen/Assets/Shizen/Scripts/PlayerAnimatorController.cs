using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [Tooltip("If null will grab from this object")]
    [SerializeField] Animator animator;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetTrigger("Idle");
    }

    void Update()
    {
        if (playerMovement.HP.IsAlive)
        {
            animator.SetBool("BackStep", playerMovement.BackStep);

            if (playerMovement.IsDashing)
            {
                StopJump();
                animator.SetTrigger("Dash");
                animator.SetBool("Falling", false);
            }
            else
            {
                if (playerMovement.IsGrounded)
                {
                    if (playerMovement.IsRunning) animator.SetTrigger("Move");
                    if (!playerMovement.IsMoving) animator.SetTrigger("Idle");
                }

                if (playerMovement.IsJumping) animator.SetBool("Jump", true);
                animator.SetBool("Falling", !playerMovement.IsGrounded);
            }
        }
        else
        {
            //animator.SetTrigger("Dead");
        }
    }

    //Can be called from animation events
    public void StopJump()
    {
        animator.SetBool("Jump", false);
        playerMovement.StopJump();
    }

    //Can be called from animation events
    public void Jump()
    {
        playerMovement.Jump();
    }
}
