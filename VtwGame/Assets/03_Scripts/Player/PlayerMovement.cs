using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private TrailRenderer tr;
    #endregion

    #region Private
    private Rigidbody2D rb;
    private PlayerInput _playerInput;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private bool climbInput = false;
    private bool isDashing;
    private bool canDash = true;
    private bool isOnBouncySurface = false;
    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
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
    #endregion

    #region Movement
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
    #endregion

    #region PlayerDirection
    private void FlipPlayerDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
    }
    #endregion

    #region Jump
    private void Jump()
    {
        if ((jumpBufferCounter > 0f || coyoteTimeCounter > 0f) && isGrounded)
        {
            float jumpForce = playerData.JumpPower;

            if (isOnBouncySurface)
            {
                jumpForce += playerData.bounceForce;
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            SoundManager.instance.PlayJumpSound();
        }
    }
    private void CancelJump(InputAction.CallbackContext context)
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private void AttemptJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Attempting to Jump. Current Action Map: {_playerInput.currentActionMap.name}");
        if (isClimbing) PerformWallJump();
        else Jump();
    }
    #endregion

    #region WallJump
    private void PerformWallJump()
    {
        float jumpDirection = transform.localScale.x > 0 ? -1 : 1;
        rb.AddForce(new Vector2(jumpDirection * playerData.wallJumpForce, playerData.JumpPower), ForceMode2D.Impulse);
        FlipPlayerDirection();
        isClimbing = false;

        SoundManager.instance.PlayJumpSound();
    }
    #endregion

    #region Buffer
    private void UpdateJumpBuffer()
    {
        if (_playerInput.actions["Jump"].triggered && !isClimbing)
        {
            jumpBufferCounter = playerData.BufferedJumpTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    #endregion

    #region CoyoteTime
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
    #endregion

    #region GroundCheck
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerData.GroundCheckRadius, playerData.GroundLayer) ||
                 Physics2D.OverlapCircle(groundCheck.position, playerData.GroundCheckRadius, LayerMask.GetMask("Bouncy"));
    }
    #endregion

    #region Dash
    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        if (canDash && playerData.AllowDash)
        {
            canDash = false;
            isDashing = true;

            SoundManager.instance.PlayDashSound();

            rb.velocity = new Vector2(horizontalInput * playerData.DashVelocity, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(playerData.DashDuration);
            tr.emitting = false;
            isDashing = false;
            yield return new WaitForSeconds(playerData.DashCooldown);
            canDash = true;
        }
    }
    #endregion

    #region EdgeClimb
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
    #endregion

    #region Climb
    private void CheckClimbingAbility()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckRight.position, playerData.checkRadius, playerData.climbableLayer);
        isClimbing = colliders.Length > 0 && climbInput;
    }

    private void Climb()
    {
        if (isClimbing)
        {
            float verticalMovement = _playerInput.actions["Movement"].ReadValue<Vector2>().y;
            rb.velocity = new Vector2(rb.velocity.x, verticalMovement * playerData.WallClimbSpeed);
        }
    }
    private void OnClimbCanceled(InputAction.CallbackContext context)
    {
        climbInput = false;
    }
    private void OnClimbPerformed(InputAction.CallbackContext context)
    {
        climbInput = true;
    }
    #endregion

    #region Bouncy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("bouncy"))
        {
            isOnBouncySurface = true;
        }
        else
        {
            isOnBouncySurface = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("bouncy"))
        {
            isOnBouncySurface = false;
        }
    }
    #endregion

    #region Enable/Disable EventHandler
    private void OnEnable()
    {
        _playerInput.actions["Movement"].performed += OnMovementPerformed;
        _playerInput.actions["Movement"].canceled += OnMovementCanceled;
        _playerInput.actions["Jump"].started += AttemptJump;
        _playerInput.actions["Jump"].canceled += CancelJump;
        _playerInput.actions["Dash"].performed += OnDashPerformed;
        _playerInput.actions["Climb"].performed += OnClimbPerformed;
        _playerInput.actions["Climb"].canceled += OnClimbCanceled;
    }
    private void OnDisable()
    {
        _playerInput.actions["Movement"].performed -= OnMovementPerformed;
        _playerInput.actions["Movement"].canceled -= OnMovementCanceled;
        _playerInput.actions["Jump"].started -= AttemptJump;
        _playerInput.actions["Jump"].canceled -= CancelJump;
        _playerInput.actions["Dash"].performed -= OnDashPerformed;
        _playerInput.actions["Climb"].performed -= OnClimbPerformed;
        _playerInput.actions["Climb"].canceled -= OnClimbCanceled;
    }
    #endregion 
}