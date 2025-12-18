using UnityEngine;

[RequireComponent(typeof(RetroRigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [Header("Sonic Ground Physics")]
    public float groundAccel = 0.046875f;   // Sonic 1
    public float groundDecel = 0.5f;        // friction
    public float topSpeed = 6f;             // Sonic 1

    [Header("Sonic Air Physics")]
    public float airAccel = 0.09375f;       // double ground accel
    public float airDrag = 0.125f;          // slows sideways in air

    [Header("Controls")]
    public KeyCode jumpKey = KeyCode.Space;

    private RetroRigidbody2D retro;

    void Awake()
    {
        retro = GetComponent<RetroRigidbody2D>();
    }

    void Update()
    {
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector2 v = retro.velocity;

        if (retro.grounded)
        {
            // --------------------------
            //       GROUND MOVEMENT
            // --------------------------

            if (input != 0)
            {
                // accelerate in input direction
                v.x += input * groundAccel;

                // clamp to Sonic top speed
                if (Mathf.Abs(v.x) > topSpeed)
                    v.x = Mathf.Sign(v.x) * topSpeed;
            }
            else
            {
                // apply friction
                if (v.x > 0)
                {
                    v.x -= groundDecel;
                    if (v.x < 0) v.x = 0;
                }
                else if (v.x < 0)
                {
                    v.x += groundDecel;
                    if (v.x > 0) v.x = 0;
                }
            }
        }
        else
        {
            // --------------------------
            //        AIR MOVEMENT
            // --------------------------

            if (input != 0)
            {
                v.x += input * airAccel;

                // air cap
                if (Mathf.Abs(v.x) > topSpeed)
                    v.x = Mathf.Sign(v.x) * topSpeed;
            }
            else
            {
                // Sonic air drag slows horizontal movement
                if (v.x > 0)
                {
                    v.x -= airDrag;
                    if (v.x < 0) v.x = 0;
                }
                else if (v.x < 0)
                {
                    v.x += airDrag;
                    if (v.x > 0) v.x = 0;
                }
            }
        }

        retro.velocity = v;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            retro.Jump();
        }
    }
}
