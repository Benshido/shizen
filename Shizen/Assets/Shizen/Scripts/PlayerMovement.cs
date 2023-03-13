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


    [SerializeField] float moveSpeed = 10;
    [SerializeField] float jumpForce = 20;
    private bool jumping = false;
    [SerializeField] float dashMultiplier = 3;
    [SerializeField] float dashDuration = 1;
    [SerializeField] float dashCooldown = 1;
    private bool dashAvailable = true;
    [SerializeField] float weight = 70;

    private CharacterController charController;
    // private Rigidbody rig;
    private Vector3 movement;
    private bool canMove = true;
    private Vector3 externalForces;
    private Vector3 dashMovement;

    public bool Moving { get { if (movement != Vector3.zero || dashMovement != Vector3.zero || jumping) return true; return false; } }

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
            Jump();
        }
        if (Input.GetKeyDown(dash) && dashAvailable)
        {
            Dash();
        }

        if (!charController.isGrounded)
        {
            if (externalForces.y > maxFallSpd)
            {
                externalForces.y = Mathf.MoveTowards(externalForces.y, maxFallSpd, 0.3f);
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
        StartCoroutine(JumpTimer());
    }

    private IEnumerator JumpTimer()
    {
        jumping = true;
        yield return new WaitForSecondsRealtime(0.1f);
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
