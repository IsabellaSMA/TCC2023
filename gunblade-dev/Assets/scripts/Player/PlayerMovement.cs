using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer playerSR;
    private PlayerAnimation playerAnimation;

    [Header("Movement Status")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallCheckDistance = 1.25f;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    private int facingDirection = 1;
    private float wallJumpCD;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isJumping;
    private bool isRunning;
    private bool jumpTrigger;
    private bool fallTrigger;
    private bool canMove = true;

    [HideInInspector] public bool CanMove { get { return canMove; } set { canMove = value; } }
    [HideInInspector] public bool IsRunning { get { return isRunning; }}
    [HideInInspector] public bool IsJumping { get { return isJumping; }}
    [HideInInspector] public bool JumpTrigger { get { return jumpTrigger; }}
    [HideInInspector] public bool FallTrigger { get { return fallTrigger; }}
    [HideInInspector] public float XSpeed { get { return playerRB.velocity.x; }}
    [HideInInspector] public float YSpeed { get { return playerRB.velocity.y; }}
    [HideInInspector] public bool IsWallJumping { get { return isWallJumping; }}

    [HideInInspector] public bool IsWallSliding { get { return isWallSliding; }}
    [HideInInspector] public bool IsGrounded { get { return GroundCheck(); }}

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }

    public void MovementUpdate()
    {
        WallSlide();

        if(Input.GetKeyDown(KeyCode.Space))
        
        jumpTrigger = true;
    }

    public void MovementFixedUpdate()
    {
        Jump(jumpTrigger);
        Move();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (!isWallSliding && !isWallJumping && canMove)
        {
            Vector2 playerVelocity = playerRB.velocity;
            playerVelocity.x = (horizontalInput * moveSpeed);
            playerRB.velocity = playerVelocity;
            if (horizontalInput > 0 && facingDirection == -1) Flip();
            else if (horizontalInput < 0 && facingDirection == 1) Flip();

            if (horizontalInput != 0) isRunning = true;
            else isRunning = false;
        }

        else isRunning = false;
    }

    void Jump(bool trigger)
    {
        if(trigger)
        {
            if (GroundCheck() && !isWallSliding)
            {
                playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                isJumping = true;
            }

            else if (!GroundCheck() && isWallSliding)
            {
                Flip();
                playerRB.velocity = new Vector2(0, 0);
                playerRB.AddForce(new Vector2(wallJumpForce.x * facingDirection, wallJumpForce.y), ForceMode2D.Impulse);

                isWallJumping = true;
            }
        }

        if (isJumping && GroundCheck())
        {
            isJumping = false;
            fallTrigger = true;
        }

        if (isWallJumping)
        {
            wallJumpCD += Time.deltaTime;

            if (WallCheck() || wallJumpCD >= 1f || GroundCheck())
            {
                isWallJumping = false;
                wallJumpCD = 0;
            }
        }

        jumpTrigger = false;
    }

    private void WallSlide()
    {
        if (WallCheck() && !GroundCheck())
        {
            isWallSliding = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        CancelSlide();
    }

    private void CancelSlide()
    {
        if(isWallSliding && Input.GetKeyDown(KeyCode.A) && facingDirection == 1)
        {
            Flip();
        }

        else if (isWallSliding && Input.GetKeyDown(KeyCode.D) && facingDirection == -1)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingDirection = -facingDirection;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private bool WallCheck()
    {
        Debug.DrawRay(transform.position, Vector3.right * wallCheckDistance * facingDirection, Color.blue);
        return Physics2D.Raycast(transform.position, transform.right * facingDirection, wallCheckDistance, wallLayer);
    }

    public bool GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green);
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }
}