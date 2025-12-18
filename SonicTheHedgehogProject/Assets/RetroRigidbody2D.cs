using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RetroRigidbody2D : MonoBehaviour
{
    [Header("Sonic Gravity Settings")]
    public float gravity = 0.21875f;
    public float terminalVelocity = -16f;
    public float jumpStrength = 6.5f;

    [Header("State")]
    public bool grounded;
    public Vector2 velocity;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        ApplyGravity();

        // finally apply our custom physics to the real Rigidbody
        rb.velocity = velocity;
    }

    void ApplyGravity()
    {
        // still falling
        if (!grounded)
        {
            velocity.y -= gravity;

            if (velocity.y < terminalVelocity)
                velocity.y = terminalVelocity;
        }
        else
        {
            // if grounded, we don't force stop the X axis
            // we only stop downward Y
            if (velocity.y < 0)
                velocity.y = 0;
        }
    }

    public void Jump()
    {
        if (!grounded) return;

        velocity.y = jumpStrength;
        grounded = false;
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
