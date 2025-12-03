using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LopezNewMovement : MonoBehaviour
{
    enum JumpState
    {
        JUMPING,
        AIRBORNE,
        FALLING,
        GROUNDED,
    }
    JumpState jState = JumpState.FALLING;

    //Rigidbody 2D
    Rigidbody2D rb;
    Vector2 moveInput;

    [Header("Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Vertical Movement")]
    // Gravity
    [SerializeField] float fallingGravityMultiplier;
    private float fallingGravity;
    [SerializeField] float airborneGravityMultiplier;
    private float airborneGravity;
    private float baseGravity;

    // Timers
    private float lastGroundedTime;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpSpeed;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float maxHoldTime;
    private float holdTimer;

    [Header("Horizontal Movement")]
    [SerializeField] float walkAccel;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] float walkDecel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        baseGravity = rb.gravityScale;
        fallingGravity = baseGravity * fallingGravityMultiplier;
        airborneGravity = baseGravity * airborneGravityMultiplier;
        holdTimer = 0;
    }

    private void Update()
    {
        GroundCheck();

        lastGroundedTime -= Time.deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        CheckJump();
    }

    private void FixedUpdate()
    {
        UpdateState();
        Run(1);
    }

    private void UpdateState()
    {
        switch (jState)
        {
            // If player is grounded
            case JumpState.GROUNDED:
                {
                    if (rb.velocity.y < 0)
                    {
                        jState = JumpState.FALLING;
                        SetGravityScale(fallingGravity);
                        break;
                    }
                    break;
                }

            // if player is falling (descending)
            case JumpState.FALLING:
                {
                    if (lastGroundedTime >= 0)
                    {
                        SetGravityScale(baseGravity);
                        jState = JumpState.GROUNDED;
                        break;
                    }

                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
                    break;
                }

            // if player is airborne (ascending but no input)
            case JumpState.AIRBORNE:
                {

                    if (rb.velocity.y < 0)
                    {
                        SetGravityScale(fallingGravity);
                        jState = JumpState.FALLING;
                        break;
                    }
                    else if (lastGroundedTime >= 0)
                    {
                        jState = JumpState.GROUNDED;
                    }
                    break;
                }

            // if player is jumping (ascending)
            case JumpState.JUMPING:
                {
                    if (rb.velocity.y <= 0)
                    {
                        EndJump();
                        SetGravityScale(fallingGravity);
                        jState = JumpState.FALLING;
                        break;
                    }
                    break;
                }
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && lastGroundedTime > 0)
        {
            StartJump();
        }
        else if (Input.GetKey(KeyCode.Space) && jState == JumpState.JUMPING)
        {
            ContinueJump();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && jState == JumpState.JUMPING)
        {
            EndJump();
        }
    }

    void StartJump()
    {
        jState = JumpState.JUMPING;
        lastGroundedTime = 0;
        holdTimer = maxHoldTime;

        float force = jumpSpeed;

        if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void ContinueJump()
    {
        holdTimer -= Time.deltaTime;
        if (holdTimer < 0 || Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
        }
    }

    void EndJump()
    {
        holdTimer = 0;
        jState = JumpState.AIRBORNE;
        SetGravityScale(airborneGravity);
    }

    private void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * maxWalkSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? walkAccel : walkDecel;

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
        
        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && jState != JumpState.JUMPING) //checks if set box overlaps with ground
        {
            jState = JumpState.GROUNDED;
            SetGravityScale(baseGravity);
            lastGroundedTime = coyoteTime; //if so sets the lastGrounded to coyoteTime
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }
}
