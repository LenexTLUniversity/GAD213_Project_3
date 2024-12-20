using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    private bool isFacingRight = true;

    public bool isWallSliding;
    private float wallSlidingSpeed = 5f;

    public bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    public Vector2 wallJumpingPower = new Vector2(12f, 30f);

    public bool isDashing = false; // New flag to check if player is dashing
    private bool canDash = true;

    public float dashingPower;
    public float dashingTime;
    public float dashingCooldown;

    private PowerUpHandler powerUpHandler;

    [SerializeField] public float speed = 15f;
    [SerializeField] public float minSpeed = 2f; // Minimum speed based on HP
    [SerializeField] public float jumpingPower = 16f;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        powerUpHandler = GetComponent<PowerUpHandler>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!isDashing) // Prevent jumping while dashing
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        // Only allow dash if dashCount > 0
        if (Input.GetKeyDown(KeyCode.LeftShift) && powerUpHandler.GetDashCount() > 0 && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing) // Only move normally if not dashing
        {
            if (!isWallJumping)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        // Reduce dashCount by 1 when dash is performed
        powerUpHandler.ReduceDashCount();

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Clamp(newSpeed, minSpeed, speed);
        Debug.Log($"Player speed set to: {speed}");
    }

}
