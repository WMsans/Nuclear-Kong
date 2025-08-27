using UnityEngine;

/// <summary>
/// Defines an area that will constrain the CameraController.
/// Requires a Collider2D component set to be a trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CameraBoundary : MonoBehaviour
{
    private CameraController cameraController;
    private Collider2D boundaryCollider;

    void Awake()
    {
        // Find the main camera in the scene and get its CameraController script.
        // This assumes you only have one main camera with the controller script.
        cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController == null)
        {
            Debug.LogError("Could not find a Camera with the CameraController script attached.", this);
            enabled = false;
            return;
        }

        // Get the collider on this object and ensure it's a trigger.
        boundaryCollider = GetComponent<Collider2D>();
        if (!boundaryCollider.isTrigger)
        {
            Debug.LogWarning("CameraBoundary's Collider2D is not set to 'Is Trigger'. Please enable it.", this);
            boundaryCollider.isTrigger = true; // Force it to be a trigger.
        }
    }

    /// <summary>
    /// Called when another collider enters this trigger.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the camera's target (the player).
        if (other.transform == cameraController.target)
        {
            // If it is, tell the camera to use this object's collider as its new boundary.
            cameraController.SetBoundary(boundaryCollider);
        }
    }

    /// <summary>
    /// Called when another collider exits this trigger.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited is the camera's target.
        if (other.transform == cameraController.target)
        {
            // If it is, tell the camera to clear its boundary.
            // This allows the camera to follow freely again.
            cameraController.ClearBoundary();
        }
    }
}
