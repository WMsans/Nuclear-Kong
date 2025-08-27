using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loop = false; // false for back-and-forth, true for loop

    private int currentWaypointIndex = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private Vector3 lastPosition;
    public Vector3 Velocity { get; private set; }

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (waypoints.Count == 0) return;

        // Move the platform
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.fixedDeltaTime);

        // Calculate velocity
        Velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

        // Check if the platform has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
        {
            if (loop)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
            else
            {
                if (currentWaypointIndex == 0)
                {
                    direction = 1;
                }
                else if (currentWaypointIndex == waypoints.Count - 1)
                {
                    direction = -1;
                }
                currentWaypointIndex += direction;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.transform.SetParent(null);
            // Add platform's momentum to the player
            player.rb.linearVelocity += new Vector2(Velocity.x, Velocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        if (loop && waypoints.Count > 1 && waypoints[^1] != null && waypoints[0] != null)
        {
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
        }

        foreach (var waypoint in waypoints)
        {
            if (waypoint != null)
            {
                Gizmos.DrawWireSphere(waypoint.position, 0.2f);
            }
        }
    }
}