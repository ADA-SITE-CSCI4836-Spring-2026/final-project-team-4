using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float sprintMultiplier = 1.8f; // how much faster on Shift
    public float jumpForce = 4f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool jumpInput;
    private bool isGrounded;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

        // SHIFT sprint
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.IsPaused || GameManager.IsGameOver) return;

        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector3 targetVelocity = moveInput * currentSpeed;

        rb.velocity = new Vector3(
            targetVelocity.x,
            rb.velocity.y,
            targetVelocity.z
        );

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

        jumpInput = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}