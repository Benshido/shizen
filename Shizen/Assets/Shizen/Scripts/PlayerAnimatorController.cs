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
    void FixedUpdate()
    {
        if (!playerMovement.IsGrounded) animator.SetTrigger("Falling");
        else
        {
            if (playerMovement.IsRunning) animator.SetTrigger("Move");
            if (!playerMovement.IsMoving) animator.SetTrigger("Idle");
        }

        if (playerMovement.IsDashing) animator.SetTrigger("Dash");


        animator.SetBool("Jump", playerMovement.IsJumping);
        if (playerMovement.IsJumping) Debug.Log("Jump");

    }
}
