using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetTrigger("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.IsGrounded)
        {
            Debug.Log("grounded");
            if (playerMovement.IsRunning) animator.SetTrigger("Move");
            if (!playerMovement.IsMoving) animator.SetTrigger("Idle");
        }

        if (playerMovement.IsDashing) animator.SetTrigger("Dash");

        if (playerMovement.IsJumping) animator.SetBool("Jump", true);
         animator.SetBool("Falling", !playerMovement.IsGrounded);

    }

    public void StopJump()
    {
        animator.SetBool("Jump", false);
        playerMovement.StopJump();
    }

    public void Jump()
    {
        playerMovement.Jump();
    }
}
