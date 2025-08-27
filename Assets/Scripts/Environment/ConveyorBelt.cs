using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private Transform movingObject;
    [SerializeField] private float speed = 2f;

    private int direction = 1; // 1 for forward, -1 for backward
    private Vector3 lastPosition;
    public Vector3 Velocity { get; private set; }
    public float CurrentSpeed
    {
        get => speed;
        set => speed = value;
    }

    private void FixedUpdate()
    {
        // Move the platform
        movingObject.position = Vector3.MoveTowards(movingObject.position, movingObject.position + transform.right * 100f, speed * Time.fixedDeltaTime);

        // Calculate velocity
        Velocity = (movingObject.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = movingObject.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.transform.SetParent(movingObject);
            // playing conveyer belt sound start with loop set to true
            if (ConveyerSounds.Instance != null)
            {
                ConveyerSounds.Instance.startSound();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.transform.SetParent(null);
            // Add platform's momentum to the player
            player.rb.linearVelocity += new Vector2(Velocity.x, Velocity.y);
            // stop conveyer belt sound
            if (ConveyerSounds.Instance != null)
            {
                ConveyerSounds.Instance.stopSound();
            }
        }
    }
}