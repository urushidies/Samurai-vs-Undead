using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset between the camera and the player
    public float smoothSpeed = 0.125f; // Smooth speed for the camera movement
    public Transform leftBoundary; // Reference to the left boundary
    public Transform rightBoundary; // Reference to the right boundary

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned to CameraFollow script.");
            return;
        }

        if (leftBoundary == null || rightBoundary == null)
        {
            Debug.LogWarning("Boundary not assigned to CameraFollow script.");
            return;
        }

        // Initialize offset if not set in the Inspector
        if (offset == Vector3.zero)
        {
            offset = new Vector3(transform.position.x - player.position.x, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (player == null || leftBoundary == null || rightBoundary == null)
        {
            return;
        }

        // Desired position considering only the x-axis and maintaining original y and z positions of the camera
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp the camera's x position between the boundaries
        float cameraHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float leftLimit = leftBoundary.position.x + cameraHalfWidth;
        float rightLimit = rightBoundary.position.x - cameraHalfWidth;

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftLimit, rightLimit);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
