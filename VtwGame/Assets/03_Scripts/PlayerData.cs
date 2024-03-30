using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float BaseSpeed = 15f;
    public float Acceleration = 100f;

    [Header("Jump")]
    public float JumpPower = 30f;
    public float BufferedJumpTime = 0.2f;
    public float CoyoteTime = 0.08f;
    public float wallJumpForce = 30f;

    [Header("Bounce")]
    public float bounceForce = 50f;

    [Header("Dash")]
    public bool AllowDash = true;
    public float DashVelocity = 50f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    [Header("Climbing")]
    public LayerMask climbableLayer;
    public float WallClimbSpeed = 5f;

    [Header("Ground Check")]
    public LayerMask GroundLayer;
    public float GroundCheckRadius = 1f;

    [Header("Edge Climb")]
    public float EdgeClimbHorizontalForce = 2f;
    public float EdgeClimbVerticalForce = 5f;
    public float checkRadius = 0.2f;

    [Header("Grab & Throw")]
    public float rayDistance = 2f;
    public float throwForce = 20f;
    public float throwDuration = 0.5f;
}
