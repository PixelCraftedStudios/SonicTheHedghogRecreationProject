using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerScript : MonoBehaviour
{
    public LayerMask groundMask;
    public float groundCheckDistance = 0.3f;

    [Header("Sonic Ground Settings")]
    public float groundAccel = 0.046875f * 60f;
    public float groundDecel = 0.5f * 60f;
    public float topSpeed = 6.0f;

    [Header("Air Settings")]
    public float airAccel = 0.09375f * 60f;
    public float airDrag = 0.96875f;

    [Header("Jump Settings")]
    public float jumpStrength = 6.5f;
    public float gravity = 0.21875f * 60f;
    public float releaseCutFactor = 0.5f;
    public float terminalVelocity = -16f;

    [Header("Slope Settings")]
    public float slopeFactor = 0.125f;

    [Header("State (Read Only)")]
    public bool grounded;
    public bool releasedJump;
    public bool rolling;
    public Vector2 velocity;
    public Vector2 groundNormal = Vector2.up;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyPhysics();
        rb.velocity = velocity;
    }

    void HandleInput()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (grounded && !rolling)
            GroundMovement(input);
        else if (!grounded)
            AirMovement(input);

        if (grounded && Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetButtonUp("Jump"))
            releasedJump = true;
    }

    void GroundMovement(float input)
    {
        if (input != 0)
        {
            velocity.x += input * groundAccel * Time.fixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -topSpeed, topSpeed);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDecel * Time.fixedDeltaTime);
        }

        float slopeAngle = Vector2.SignedAngle(groundNormal, Vector2.up);
        float rad = slopeAngle * Mathf.Deg2Rad;
        velocity.x += Mathf.Sin(rad) * -slopeFactor;
    }

    void AirMovement(float input)
    {
        if (input != 0)
            velocity.x += input * airAccel * Time.fixedDeltaTime;

        velocity.x *= airDrag;
    }

    void ApplyPhysics()
    {
        if (!grounded)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            if (velocity.y < terminalVelocity)
                velocity.y = terminalVelocity;
        }

        if (releasedJump && velocity.y > 0)
            velocity.y *= releaseCutFactor;
    }

    void Jump()
    {
        velocity.y = jumpStrength;
        grounded = false;
        releasedJump = false;
    }

    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,
            groundCheckDistance, groundMask);

        bool wasGrounded = grounded;
        grounded = hit.collider != null;

        if (grounded)
        {
            groundNormal = hit.normal;
            if (!wasGrounded && velocity.y < 0)
                velocity.y = 0;
        }
        else
        {
            groundNormal = Vector2.up;
        }
    }
}
