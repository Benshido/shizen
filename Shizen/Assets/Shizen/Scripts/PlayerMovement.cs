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
    [SerializeField] float sprintIncrease = 5;

    [Header("Jump")]
    [SerializeField] float jumpForce = 20;

    private bool jumping = false;
    public bool IsJumping { get { return jumping; } }
   // private bool inCombat = false;
  //  public bool InCombat { get { return inCombat; } }

    [Header("Dash")]
    [SerializeField] float dashMultiplier = 3;
    [SerializeField] float dashDuration = 1;
    [SerializeField] float dashCooldown = 1;
    [SerializeField] float dashCost = 1;
    /// <summary>
    /// Will be set to false on knockbacks
    /// </summary>
    private bool dashAvailable = true;
    private bool backStep = false;
    public bool BackStep { get { return backStep; } }
    [Header("Other")]
    [SerializeField] float weight = 70;
    [SerializeField] float timeUntillSprint = 4;
    [SerializeField] GameObject orient;
    private float sprintTimer = 0;

    private CharacterController charController;

    [SerializeField] bool isGrounded = true;
    [SerializeField] LayerMask groundedMask;
    public bool IsGrounded { get { return isGrounded; } }

    private Vector3 slopeSlideVelo;
    private float slopeAngle;
    private bool IsSliding { get { return slopeAngle > charController.slopeLimit + 1; } }

    private Vector3 finalMovement;
    private Vector3 movement;
    public Vector3 Movement { get { return finalMovement; } }
    public bool Can_Move { get { return canMove; } }
    private bool canMove = true;
    private bool isStaggered = false;
    private Vector3 externalForces;
    public Vector3 ExternalForces { get { return externalForces; } }
    private Vector3 dashMovement;
    public Vector3 DashMovement { get { return dashMovement; } }
    public float SprintAmount { get { return sprintAmount; } }
    private float sprintAmount = 0;

    public bool IsMoving { get { if (IsRunning || IsDashing || IsJumping) return true; return false; } }
    public bool IsRunning { get { if (movement != Vector3.zero) return true; return false; } }
    public bool IsDashing { get { if (dashMovement != Vector3.zero) return true; return false; } }
    public PlayerHP HP { get { return stats; } }
    private PlayerHP stats;

    private float maxFallSpd = 0;
    private int dashCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        stats = GetComponentInChildren<PlayerHP>();

        //calculate maximum fall speed using gravity and weight
        maxFallSpd = Physics.gravity.y + (Physics.gravity.y * (weight / 75));

        externalForces.y = maxFallSpd;
    }


    // Update is called once per frame
    void Update()
    {
        // SetSlopeSlideVelocity();
        slopeSlideVelo = Vector3.Lerp(slopeSlideVelo, Vector3.zero, Time.unscaledDeltaTime * 5);

        //grounded check (character controller isGrounded is too glitchy)
        Vector3 origin = transform.position + charController.center;
        var radius = charController.radius * 0.99f;
        origin.y -= (charController.height / 2) - (radius / 2);

        var sphere = Physics.OverlapSphere(origin, radius, groundedMask);
        int noTriggers = 0;

        bool hasANonSlope = false;
        for (int i = 0; i < sphere.Length; i++)
            if (!sphere[i].isTrigger)
            {
                if (!hasANonSlope)
                {
                    Vector3 end = new();
                    bool meshcolConvex = false;
                    if (sphere[i] is MeshCollider)
                    {
                        var col = (MeshCollider)sphere[i];
                        meshcolConvex = col.convex;
                    }
                    //ClosestPoint does not work with non-convex mesh colliders and terrain colliders
                    if (sphere[i] is not TerrainCollider && meshcolConvex)
                    {
                        end = sphere[i].ClosestPoint(origin);
                    }
                    else
                    {
                        RaycastHit hit1;
                        float dist = 0;
                        //cast multiple rays to roughly find the closest part of the terrain
                        Ray rayD = new(origin, -transform.up);
                        Ray rayF = new(origin, transform.forward);
                        Ray rayR = new(origin, transform.right);
                        Ray rayL = new(origin, -transform.right);

                        if (Physics.Raycast(rayD, out hit1))
                        {
                            dist = hit1.distance;
                            end = hit1.point;
                        }
                        if (Physics.Raycast(rayF, out hit1))
                        {
                            if (hit1.distance < dist)
                            {
                                dist = hit1.distance;
                                end = hit1.point;
                            }
                        }
                        if (Physics.Raycast(rayR, out hit1))
                        {
                            if (hit1.distance < dist)
                            {
                                dist = hit1.distance;
                                end = hit1.point;
                            }
                        }
                        if (Physics.Raycast(rayL, out hit1))
                        {
                            if (hit1.distance < dist)
                            {
                                dist = hit1.distance;
                                end = hit1.point;
                            }
                        }
                    }
                    var start = end;
                    start.y += 0.1f;
                    var direction = end - start;

                    RaycastHit hit;
                    if (Physics.Raycast(start, direction, out hit))
                    {
                        slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                        if (slopeAngle >= charController.slopeLimit)
                        {
                            slopeSlideVelo = hit.normal;
                        }
                        else hasANonSlope = true;
                    }
                }
                noTriggers++;
            }

        if (noTriggers > 0 && !IsSliding)
        {
            isGrounded = true;
        }
        else isGrounded = false;

        if (stats.IsAlive)
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

            if (IsRunning && canMove)
            {
                sprintTimer += Time.unscaledDeltaTime;

                if (sprintTimer >= timeUntillSprint)
                {
                    if (sprintTimer >= timeUntillSprint * 2) sprintAmount = Mathf.MoveTowards(sprintAmount, 1f, Time.unscaledDeltaTime);
                    else sprintAmount = Mathf.MoveTowards(sprintAmount, 0.5f, Time.unscaledDeltaTime);
                }
            }
            else
            {
                sprintTimer = 0;
                sprintAmount = 0;
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

            if (Input.GetKeyDown(dash) && dashAvailable && stats.ComsumeStamina(dashCost))
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
            if (IsDashing) externalForces.y = 0;

            //smooth movement
            if (isGrounded || IsDashing) finalMovement = Vector3.MoveTowards(finalMovement, movement, Time.unscaledDeltaTime * 30);
            else finalMovement = Vector3.MoveTowards(finalMovement, movement, Time.unscaledDeltaTime * 15);


            var movementAndDir = Vector3.zero;
            if (!isStaggered) movementAndDir = orient.transform.rotation * (finalMovement + dashMovement);
            if (!canMove) movementAndDir = orient.transform.rotation * dashMovement;

            if (IsSliding && !IsDashing)
            {
                float initMovSpdDivider = 20;
                movementAndDir = new Vector3(slopeSlideVelo.x + movementAndDir.x / initMovSpdDivider, -slopeSlideVelo.y, slopeSlideVelo.z + movementAndDir.z / initMovSpdDivider) * 6;
            }

            //External force can be used for jumping and knockbacks
            charController.Move((externalForces + movementAndDir) * Time.unscaledDeltaTime);
        }

      /*  //quick test on time slowing/speeding up
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0.01f;
            else { Time.timeScale = 1; }
        }*/
    }

    /* private void SetSlopeSlideVelocity()
     {
         var origin = transform.position - charController.center;

          if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, charController.height, groundedMask))
          {
              slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
              if (slopeAngle >= charController.slopeLimit)
              {
                  slopeSlideVelo = hit.normal;
                  return;
              }
          }
         slopeSlideVelo = Vector3.MoveTowards(slopeSlideVelo, Vector3.zero, Time.unscaledDeltaTime);
     }*/

    public void MoveForward()
    {
        movement.z = moveSpeed + (sprintIncrease * sprintAmount);
    }

    public void MoveBack()
    {
        movement.z = -(moveSpeed + (sprintIncrease * sprintAmount));
    }

    public void MoveLeft()
    {
        movement.x = -(moveSpeed + (sprintIncrease * sprintAmount));
    }

    public void MoveRight()
    {
        movement.x = moveSpeed + (sprintIncrease * sprintAmount);
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

        HP.IFrame(0.1f);
        //If not moving, the backstep will be used instead
        if (dashMovement == Vector3.zero)
        {
            dashMovement.z = -(moveSpeed * dashMultiplier);
            backStep = true;
        }
        StartCoroutine(DashForceReset());
    }

    public void CanMove(int value)
    {
        if (value == 0) canMove = false;
        else canMove = true;
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
