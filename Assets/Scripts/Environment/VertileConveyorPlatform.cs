using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class VertileConveyorPlatform : MonoBehaviour
{
    /// <summary>
    /// A list of transforms representing the platforms attached to the belt.
    /// </summary>
    [Tooltip("The platforms that will move along the conveyor belt.")]
    public List<Transform> platforms;

    /// <summary>
    /// The speed at which the platforms rotate around the conveyor belt.
    /// </summary>
    [Tooltip("The speed of the conveyor belt.")]
    public float speed = 2f;

    private CapsuleCollider2D capsuleCollider;
    private Vector2 topEndpoint;
    private Vector2 bottomEndpoint;
    private float radius;
    private List<float> platformAngles;

    /// <summary>
    /// Initializes the component by getting the CapsuleCollider2D and setting up the platforms.
    /// </summary>
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        UpdateEndpointPositions();
    }

    /// <summary>
    /// Updates the position of each platform every frame.
    /// </summary>
    void FixedUpdate()
    {
        UpdateEndpointPositions();
        MovePlatforms();
    }

    /// <summary>
    /// Updates the world positions of the capsule's top and bottom endpoints.
    /// </summary>
    private void UpdateEndpointPositions()
    {
        Vector2 center = transform.TransformPoint(capsuleCollider.offset);
        float height = capsuleCollider.size.y - (capsuleCollider.size.x); // Adjusted height for the straight part
        radius = capsuleCollider.size.x / 2f;

        // Assumes a vertical capsule
        topEndpoint = center + Vector2.up * (height / 2f);
        bottomEndpoint = center - Vector2.up * (height / 2f);
    }

    /// <summary>
    /// Moves and rotates the platforms along the conveyor belt path.
    /// </summary>
    private void MovePlatforms()
    {
        if (platforms == null) return;

        foreach (var platform in platforms)
        {
            Vector2 endPoint;
            if (platform.position.y > topEndpoint.y)
            {
                endPoint = topEndpoint;
            }
            else if (platform.position.y < bottomEndpoint.y)
            {
                endPoint = bottomEndpoint;
            }
            else
            {
                endPoint = new(transform.TransformPoint(capsuleCollider.offset).x, platform.position.y);
            }
            var hit = Physics2D.Raycast(platform.position, (endPoint - (Vector2)platform.position).normalized, Mathf.Infinity, 1 << gameObject.layer);
            if(!hit) continue;
            var normal = hit.normal.normalized;
            platform.rotation = Quaternion.Euler(platform.eulerAngles.x, platform.eulerAngles.y, Mathf.Atan2( normal.y, normal.x ) * Mathf.Rad2Deg);
            platform.position = hit.point;
            platform.position += (Vector3)(Vector2.Perpendicular(normal) * speed);
        }
    }
}