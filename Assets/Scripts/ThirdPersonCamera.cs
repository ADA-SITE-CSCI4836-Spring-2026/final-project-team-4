using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 4f;
    public float height = 2f;
    public float mouseSensitivity = 3f;

    public float minVerticalAngle = -10f;
    public float maxVerticalAngle = 35f;

    public LayerMask collisionLayers;
    public float cameraRadius = 0.25f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (PauseManager.IsPaused) return;
        if (GameManager.IsGameOver) return;
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * distance;

        Vector3 direction = desiredPosition - targetPosition;

        float finalDistance = distance;

        if (Physics.SphereCast(
            targetPosition,
            cameraRadius,
            direction.normalized,
            out RaycastHit hit,
            distance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            finalDistance = Mathf.Clamp(hit.distance - 0.2f, 1f, distance);
        }

        Vector3 finalPosition = targetPosition - rotation * Vector3.forward * finalDistance;

        transform.position = finalPosition;
        transform.LookAt(targetPosition);
    }
}