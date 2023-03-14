using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Key Binds")]
    [SerializeField]
    KeyCode moveForward = KeyCode.W,
        moveBack = KeyCode.S,
        moveLeft = KeyCode.A,
        moveRight = KeyCode.D,
        jump = KeyCode.Space,
        dash = KeyCode.LeftShift;

    [Header("Move Speed")]
    [SerializeField] float moveSpeed = 10;

    [Header("Jump")]
    [SerializeField] float jumpForce = 20;

    private bool jumping = false;
    public bool IsJumping { get { return jumping; } }

    [Header("Dash")]
    [SerializeField] float dashMultiplier = 3;
    [SerializeField] float dashDuration = 1;
    [SerializeField] float dashCooldown = 1;
    private bool dashAvailable = true;
    [Header("Other")]
    [SerializeField] float weight = 70;

    private CharacterController charController;

    [SerializeField] bool isGrounded = true;
    [SerializeField] LayerMask groundedMask;
    public bool IsGrounded { get { return isGrounded; } }

    // private Rigidbody rig;
    private Vector3 movement;
    public Vector3 Movement { get { return movement; } }
    private bool canMove = true;
    private Vector3 externalForces;
    private Vector3 dashMovement;

    public bool IsMoving { get { if (IsRunning || IsDashing || IsJumping) return true; return false; } }
    public bool IsRunning { get { if (movement != Vector3.zero) return true; return false; } }
    public bool IsDashing { get { if (dashMovement != Vector3.zero) return true; return false; } }

    private float maxFallSpd = 0;
    private int dashCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        maxFallSpd = Physics.gravity.y + (Physics.gravity.y * (weight / 75));

        externalForces.y = maxFallSpd;
    }

    // Update is called once per frame
    void Update()
    {
        //grounded check (character controller isGrounded is too glitchy)
        Vector3 origin = transform.position + charController.center;
        origin.y -= (charController.height / 2) - 0.03f;
        if (Physics.Raycast(origin, -transform.up, 0.3f, groundedMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;
        Debug.DrawRay(origin, -transform.up, Color.red);


        movement = Vector3.zero;
        if (Input.GetKey(moveForward))
        {
            MoveForward();
        }
        if (Input.GetKey(moveBack))
        {
            MoveBack();
        }
        if (Input.GetKey(moveLeft))
        {
            MoveLeft();
        }
        if (Input.GetKey(moveRight))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(jump) && charController.isGrounded)
        {
            jumping = true;
            //Jump();
           // StartCoroutine(JumpTimer());
        }
        if (Input.GetKeyDown(dash) && dashAvailable)
        {
            Dash();
        }

        if (!isGrounded)
        {
            if (externalForces.y > maxFallSpd)
            {
                externalForces.y = Mathf.MoveTowards(externalForces.y, maxFallSpd, Time.unscaledDeltaTime * 60);
            }
        }
        else if (!jumping) externalForces.y = maxFallSpd / 5;

        if (dashCount > 0) externalForces.y = 0;

        var movementAndDir = Vector3.zero;
        if (canMove) movementAndDir = transform.rotation * (movement + dashMovement);

        charController.Move((externalForces + movementAndDir) * Time.unscaledDeltaTime);

        if (Input.GetKeyDown(KeyCode.Q)) Time.timeScale = 0.02f;
        if (Input.GetKeyDown(KeyCode.E)) Time.timeScale = 1;
    }

    public void MoveForward()
    {
        movement.z = moveSpeed;
    }

    public void MoveBack()
    {
        movement.z = -moveSpeed;
    }

    public void MoveLeft()
    {
        movement.x = -moveSpeed;
    }

    public void MoveRight()
    {
        movement.x = moveSpeed;
    }

    public void Jump()
    {
        externalForces.y = jumpForce;
        // StartCoroutine(JumpTimer());
    }

   /* private IEnumerator JumpTimer()
    {
        jumping = true;
        yield return new WaitForEndOfFrame();
        yield return 1;
        jumping = false;
    }
   */
   public void StopJump()
    {
        jumping = false;
    }

    public void Dash()
    {
        dashMovement = movement * dashMultiplier;
        if (dashMovement == Vector3.zero) dashMovement.z = -(moveSpeed * dashMultiplier);
        StartCoroutine(DashForceReset());
    }

    private IEnumerator DashForceReset()
    {
        dashCount++;
        StartCoroutine(DashCooldown());
        yield return new WaitForSecondsRealtime(dashDuration);

        dashCount--;
        if (dashCount == 0) dashMovement = Vector3.zero;
    }

    private IEnumerator DashCooldown()
    {
        dashAvailable = false;
        yield return new WaitForSecondsRealtime(dashCooldown);
        dashAvailable = true;
    }
}
