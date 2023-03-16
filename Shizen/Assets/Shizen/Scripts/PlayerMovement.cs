using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
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
    /// <summary>
    /// Will be set to false on knockbacks
    /// </summary>
    private bool dashAvailable = true;
    private bool backStep = false;
    public bool BackStep { get { return backStep; } }
    [Header("Other")]
    [SerializeField] float weight = 70;

    private CharacterController charController;

    [SerializeField] bool isGrounded = true;
    [SerializeField] LayerMask groundedMask;
    public bool IsGrounded { get { return isGrounded; } }

    private Vector3 finalMovement;
    private Vector3 movement;
    public Vector3 Movement { get { return finalMovement; } }
    private bool canMove = true;
    private Vector3 externalForces;
    public Vector3 ExternalForces { get { return externalForces; } }
    private Vector3 dashMovement;
    public Vector3 DashMovement { get { return dashMovement; } }

    public bool IsMoving { get { if (IsRunning || IsDashing || IsJumping) return true; return false; } }
    public bool IsRunning { get { if (movement != Vector3.zero) return true; return false; } }
    public bool IsDashing { get { if (dashMovement != Vector3.zero) return true; return false; } }
    public PlayerHP HP { get { return hp; } }
    private PlayerHP hp;

    private float maxFallSpd = 0;
    private int dashCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        hp = GetComponentInChildren<PlayerHP>();

        //calculate maximum fall speed using gravity and weight
        maxFallSpd = Physics.gravity.y + (Physics.gravity.y * (weight / 75));

        externalForces.y = maxFallSpd;
    }

    // Update is called once per frame
    void Update()
    {
        //grounded check (character controller isGrounded is too glitchy)
        Vector3 origin = transform.position + charController.center;
        origin.y -= (charController.height / 2) - 0.03f;
        if (Physics.OverlapSphere(origin, charController.radius, groundedMask).Length > 0)
        {
            isGrounded = true;
        }
        else isGrounded = false;

        if (hp.IsAlive)
        {
            //reset movement before trying to see if we should be moving
            movement = Vector3.zero;

            //when oposite keys are pressed at the same time it should act like neither are pressed
            if (Input.GetKey(moveForward) && !Input.GetKey(moveBack))
            {
                MoveForward();
            }
            if (Input.GetKey(moveBack) && !Input.GetKey(moveForward))
            {
                MoveBack();
            }
            if (Input.GetKey(moveLeft) && !Input.GetKey(moveRight))
            {
                MoveLeft();
            }
            if (Input.GetKey(moveRight) && !Input.GetKey(moveLeft))
            {
                MoveRight();
            }

            //evens out movement when multiple directions are being moved towards
            float movementDivider = 1.5f;
            if (movement.x != 0) movement.z /= movementDivider;
            if (movement.z != 0) movement.x /= movementDivider;

            //Player can only jump when grounded
            if (Input.GetKeyDown(jump) && isGrounded)
            {
                jumping = true;
            }

            if (Input.GetKeyDown(dash) && dashAvailable)
            {
                Dash();
            }

            if (!isGrounded)
            {
                //increases fall speed over time when falling
                if (externalForces.y > maxFallSpd)
                {
                    externalForces.y = Mathf.MoveTowards(externalForces.y, maxFallSpd, Time.unscaledDeltaTime * 60);
                }
            }
            //The player needs to have some downward force while grounded for slopes when not trying to jump
            else if (!jumping) externalForces.y = maxFallSpd / 3;

            //When dashing the player should not fall
            if (dashCount > 0) externalForces.y = 0;

            //smooth movement
            if (isGrounded || IsDashing) finalMovement = Vector3.MoveTowards(finalMovement, movement, Time.unscaledDeltaTime * 30);
            else finalMovement = Vector3.MoveTowards(finalMovement, movement, Time.unscaledDeltaTime * 15);


            var movementAndDir = Vector3.zero;
            if (canMove) movementAndDir = transform.rotation * (finalMovement + dashMovement);

            //External force can be used for jumping and knockbacks
            charController.Move((externalForces + movementAndDir) * Time.unscaledDeltaTime);
        }

        //quick test on time slowing/speeding up
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
    }

    public void StopJump()
    {
        jumping = false;
    }

    public void Dash()
    {
        dashMovement = movement * dashMultiplier;

        //If not moving, the backstep will be used instead
        if (dashMovement == Vector3.zero)
        {
            dashMovement.z = -(moveSpeed * dashMultiplier);
            backStep = true;
        }
        StartCoroutine(DashForceReset());
    }

    /// <summary>
    /// Sets DashForce to Vector3.zero
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashForceReset()
    {
        dashCount++;
        StartCoroutine(DashCooldown());
        yield return new WaitForSecondsRealtime(dashDuration);

        dashCount--;
        if (dashCount == 0) { dashMovement = Vector3.zero; backStep = false; }
    }

    private IEnumerator DashCooldown()
    {
        dashAvailable = false;
        yield return new WaitForSecondsRealtime(dashCooldown + dashDuration);
        dashAvailable = true;
    }
}
