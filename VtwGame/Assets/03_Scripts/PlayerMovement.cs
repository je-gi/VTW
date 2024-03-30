using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D rb;
    private PlayerControls controls;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private bool climbInput = false;
    private bool isDashing;
    private bool canDash = true;
    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeControls();
    }

    private void InitializeControls()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Movement.performed += ctx => OnMovementPerformed(ctx);
        controls.Player.Movement.canceled += ctx => OnMovementCanceled(ctx);
        controls.Player.Jump.started += _ => AttemptJump();
        controls.Player.Jump.canceled += _ => CancelJump();
        controls.Player.Dash.performed += _ => StartCoroutine(Dash());
        controls.Player.Climb.performed += _ => climbInput = true;
        controls.Player.Climb.canceled += _ => climbInput = false;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        horizontalInput = movement.x;
        verticalInput = movement.y;
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        if (!isClimbing) Move();
        else if (climbInput) Climb();
    }

    private void Update()
    {
        UpdateJumpBuffer();
        UpdateCoyoteTime();
        CheckClimbingAbility();
        AttemptEdgeClimb();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("bouncy")) Bounce();
    }

    private void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerData.bounceForce);
    }

    private void AttemptJump()
    {
        if (isClimbing) PerformWallJump();
        else Jump();
    }

    private void PerformWallJump()
    {
        float jumpDirection = transform.localScale.x > 0 ? -1 : 1;
        rb.AddForce(new Vector2(jumpDirection * playerData.wallJumpForce, playerData.JumpPower), ForceMode2D.Impulse);
        FlipPlayerDirection();
        isClimbing = false;
    }

    private void FlipPlayerDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
    }

    private IEnumerator Dash()
    {
        if (canDash && playerData.AllowDash)
        {
            canDash = false;
            isDashing = true;
            rb.velocity = new Vector2(horizontalInput * playerData.DashVelocity, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(playerData.DashDuration);
            tr.emitting = false;
            isDashing = false;
            yield return new WaitForSeconds(playerData.DashCooldown);
            canDash = true;
        }
    }

    private void AttemptEdgeClimb()
    {
        if (isClimbing && !Physics2D.OverlapCircle(edgeCheck.position, playerData.checkRadius, playerData.climbableLayer))
        {
            Collider2D wallCheck = Physics2D.OverlapCircle(wallCheckRight.position, playerData.checkRadius, playerData.climbableLayer);
            if (wallCheck != null) ClimbOverEdge();
        }
    }

    private void ClimbOverEdge()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 boostForce = new Vector2(direction * playerData.EdgeClimbHorizontalForce, playerData.EdgeClimbVerticalForce);
        rb.AddForce(boostForce, ForceMode2D.Impulse);
        StartCoroutine(EndClimbAfterDelay());
    }

    private IEnumerator EndClimbAfterDelay(float delay = 0.2f)
    {
        yield return new WaitForSeconds(delay);
        isClimbing = false;
        climbInput = false;
    }

    private void Move()
    {
        float targetSpeed = horizontalInput * playerData.BaseSpeed;
        float movementSpeed = Mathf.MoveTowards(rb.velocity.x, targetSpeed, playerData.Acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);

        if (Mathf.Abs(horizontalInput) > 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1f, 1f);
        }
    }

    private void Jump()
    {
        if ((jumpBufferCounter > 0f || coyoteTimeCounter > 0f) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerData.JumpPower);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void CancelJump()
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void CheckClimbingAbility()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckRight.position, playerData.checkRadius, playerData.climbableLayer);
        isClimbing = colliders.Length > 0 && climbInput;
    }

    private void Climb()
    {
        if (isClimbing)
        {
            float verticalMovement = controls.Player.Movement.ReadValue<Vector2>().y;
            rb.velocity = new Vector2(rb.velocity.x, verticalMovement * playerData.WallClimbSpeed);
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerData.GroundCheckRadius, playerData.GroundLayer);
    }

    private void UpdateJumpBuffer()
    {
        if (controls.Player.Jump.triggered && !isClimbing)
        {
            jumpBufferCounter = playerData.BufferedJumpTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = playerData.CoyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}