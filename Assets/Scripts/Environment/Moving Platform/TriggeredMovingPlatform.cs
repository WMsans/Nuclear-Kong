using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controls a moving platform that is activated by a trigger.
/// This version uses FixedUpdate for smooth, physics-based movement.
/// A Rigidbody2D component (set to Kinematic) is required on the platform.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class TriggeredMovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveTime = 2f;
    [SerializeField] private bool autoReturn = false;

    [Header("References")]
    [SerializeField] private MovingPlatformTriggerZone trigger;

    // State & Velocity
    private Vector3 _velocity;
    private Vector3 _lastPosition;
    private bool _isAtEnd = false;
    private bool _isMoving = false;
    
    // Movement Calculation
    private float _elapsedTime;
    private float _currentDuration;
    private Vector3 _startPositionForMovement;
    private Vector3 _targetPosition;
    private System.Func<float, float> _currentEaseFunction;

    // Momentum
    private readonly List<KeyValuePair<float, Vector2>> _speedHistory = new();
    private const float MomentumGracePeriod = 0.3f;
    private void Start()
    {
        if (trigger != null)
        {
            trigger.OnPlayerEnter += HandlePlayerEnter;
        }
        transform.position = startPoint.position;
        _lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        if (trigger != null)
        {
            trigger.OnPlayerEnter -= HandlePlayerEnter;
        }
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
        {
            _velocity = Vector3.zero;
            _speedHistory.Clear();
            _lastPosition = transform.position;
            return;
        }

        // --- Perform Movement ---
        _elapsedTime += Time.fixedDeltaTime;
        float progress = Mathf.Clamp01(_elapsedTime / _currentDuration);
        float easedProgress = _currentEaseFunction(progress);

        // Use MovePosition for smooth, physics-based movement
        transform.position = Vector3.Lerp(_startPositionForMovement, _targetPosition, easedProgress);

        // --- Calculate Velocity ---
        _velocity = ((Vector2)transform.position - (Vector2)_lastPosition) / Time.fixedDeltaTime;
        _speedHistory.Add(new KeyValuePair<float, Vector2>(Time.time, _velocity));
        _lastPosition = transform.position;
        _speedHistory.RemoveAll(kvp => Time.time - kvp.Key > MomentumGracePeriod);

        // --- Check for Completion ---
        if (progress >= 1f)
        {
            _isMoving = false;
            transform.position = _targetPosition; // Snap to final position

            // Handle state changes and auto-return
            if (_targetPosition == endPoint.position)
            {
                _isAtEnd = true;
                if (autoReturn)
                {
                    MoveToStart(true);
                }
            }
            else
            {
                _isAtEnd = false;
            }
        }
    }

    /// <summary>
    /// Called when the player enters the trigger zone.
    /// </summary>
    private void HandlePlayerEnter()
    {
        if (_isMoving)
        {
            // If it's auto-returning, interrupt and go back to the end
            if (autoReturn && _targetPosition == startPoint.position)
            {
                MoveToEnd();
            }
            return;
        }

        if (!_isAtEnd)
        {
            MoveToEnd();
        }
        else
        {
            MoveToStart();
        }
    }

    /// <summary>
    /// Sets up the state for movement towards the end point.
    /// </summary>
    private void MoveToEnd()
    {
        _startPositionForMovement = transform.position;
        _targetPosition = endPoint.position;
        _currentDuration = moveTime;
        _currentEaseFunction = EaseInOutQuad;
        _elapsedTime = 0f;
        _isMoving = true;
    }

    /// <summary>
    /// Sets up the state for movement towards the start point.
    /// </summary>
    private void MoveToStart(bool isAuto = false)
    {
        _startPositionForMovement = transform.position;
        _targetPosition = startPoint.position;
        _currentDuration = isAuto ? Vector3.Distance(transform.position, startPoint.position) / 5f : moveTime;
        _currentEaseFunction = isAuto ? Linear : EaseInOutQuad;
        _elapsedTime = 0f;
        _isMoving = true;
    }

    #region Easing Functions
    private float Linear(float t) => t;
    private float EaseInOutQuad(float t) => t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    #endregion

    #region Collision Handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out _))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.transform.SetParent(null);

            Vector2 maxSpeed = Vector2.zero;
            if (_speedHistory.Any())
            {
                maxSpeed = _speedHistory.OrderByDescending(kvp => kvp.Value.sqrMagnitude).First().Value;
            }
            
            if(player.rb != null)
            {
                player.rb.linearVelocity += maxSpeed * .35f;
            }
        }
    }
    #endregion
}