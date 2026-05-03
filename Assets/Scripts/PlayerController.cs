using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveForceGround = 120f;
    public float moveForceAir = 30f;
    public float sprintMultiplier = 1.6f;
    public float jumpForce = 50f;
    public float rotationSpeed = 10f;

    public float extraGravity = 50f;

    public float maxGroundSpeed = 30f;
    public float maxSprintSpeed = 45f;
    public float maxAirSpeed = 30f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool jumpInput;
    private bool isGrounded;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (PauseManager.IsPaused || GameManager.IsGameOver) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        moveInput = (camForward * v + camRight * h).normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.IsPaused || GameManager.IsGameOver) return;

        float currentForce = isGrounded ? moveForceGround : moveForceAir;

        if (isSprinting)
        {
            currentForce *= sprintMultiplier;
        }

        Vector3 horizontalForce = moveInput * currentForce;
        rb.AddForce(horizontalForce, ForceMode.Force);

        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }

        if (jumpInput && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }

        LimitHorizontalSpeed();

        // if (isGrounded && moveInput.magnitude < 0.1f)
        // {
        //     Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //     horizontalVel *= 0.9f;
        //     rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        // }
        // Smooth deceleration only when really standing still
        if (isGrounded && moveInput.magnitude < 0.1f)
        {
            rb.drag = 2f;
        }
        else
        {
            rb.drag = 0.8f;
        }

        jumpInput = false;
    }

    void LimitHorizontalSpeed()
    {
        Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float maxSpeed = isGrounded ? maxGroundSpeed : maxAirSpeed;

        if (isGrounded && isSprinting)
        {
            maxSpeed = maxSprintSpeed;
        }

        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}