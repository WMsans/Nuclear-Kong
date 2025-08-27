using UnityEngine;

/// <summary>
/// Controls the 2D camera to smoothly follow a target (e.g., the player).
/// It can also be confined within a specified boundary.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Target to Follow")]
    [Tooltip("The object the camera will follow. Assign your player here.")]
    public Transform target;

    public bool locateCameraOnStart;

    [Header("Movement Settings")]
    [Tooltip("How quickly the camera catches up to the target. Smaller values are slower/smoother.")]
    public float smoothSpeed = 0.125f;

    [Tooltip("The offset from the target's position. Keep Z at -10 for a standard 2D camera.")]
    public Vector3 offset = new Vector3(0, 0, -10);

    // This will hold the bounds of the current active boundary collider.
    private Bounds currentBoundary;
    private bool isBounded = false;

    private Camera cam;

    void Start()
    {
        // Get a reference to the Camera component to calculate its view dimensions.
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController requires a Camera component on the same GameObject.", this);
            enabled = false; // Disable the script if no camera is found.
            return;
        }

        if (locateCameraOnStart && target)
        {
            transform.position = target.position;
        }
    }

    /// <summary>
    /// LateUpdate is called after all Update functions have been called.
    /// This is the best place to move the camera, as it ensures the target has
    /// finished its movement for the frame.
    /// </summary>
    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not assigned.", this);
            return;
        }

        // Calculate the initial desired position based on the target and offset.
        Vector3 desiredPosition = target.position + offset;

        // --- MODIFIED LOGIC ---
        // If the camera is supposed to be bounded, we clamp the *desired position*
        // before the smoothing calculation. This ensures the camera smoothly
        // moves towards a valid point within the boundary.
        if (isBounded)
        {
            // Calculate the camera's half-width and half-height.
            // This is crucial for ensuring the *view* of the camera stays within the bounds.
            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = cam.aspect * camHalfHeight;

            // Clamp the desired position to the boundary limits.
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, currentBoundary.min.x + camHalfWidth, currentBoundary.max.x - camHalfWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, currentBoundary.min.y + camHalfHeight, currentBoundary.max.y - camHalfHeight);
        }

        // Use linear interpolation (Lerp) to smoothly move from the current position
        // to the (now potentially clamped) desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.unscaledDeltaTime);

        // Apply the calculated smoothed position to the camera.
        transform.position = smoothedPosition;
    }

    /// <summary>
    /// This public method is called by the CameraBoundary script to set a new boundary.
    /// </summary>
    /// <param name="newBoundary">The Collider2D defining the new boundary area.</param>
    public void SetBoundary(Collider2D newBoundary)
    {
        if (newBoundary != null)
        {
            currentBoundary = newBoundary.bounds;
            isBounded = true;
        }
    }

    /// <summary>
    /// This public method is called by the CameraBoundary script to remove the boundary.
    /// </summary>
    public void ClearBoundary()
    {
        isBounded = false;
    }
}
