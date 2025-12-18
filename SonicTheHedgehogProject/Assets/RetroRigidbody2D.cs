using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RetroRigidbody2D : MonoBehaviour
{
    [Header("Sonic Gravity Settings")]
    public float gravity = 0.21875f;      // classic sonic fall accel
    public float terminalVelocity = -16f; // max fall speed
    public float jumpStrength = 6.5f;

    [Header("State")]
    public bool grounded;
    public Vector2 velocity;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;  // disable Unity physics gravity
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        rb.velocity = velocity;
    }

    void ApplyGravity()
    {
        if (grounded) return;

        velocity.y -= gravity;

        if (velocity.y < terminalVelocity)
            velocity.y = terminalVelocity;
    }

    public void Jump()
    {
        if (!grounded) return;
        velocity.y = jumpStrength;
        grounded = false;
    }

    // you call this from a ground check script
    public void SetGrounded(bool state)
    {
        grounded = state;

        if (state && velocity.y < 0)
            velocity.y = 0;
    }
}
