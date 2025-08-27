using System;
using UnityEngine;
using Water2D;

public class WaterLevelManager : MonoSingleton<WaterLevelManager>, IResetable
{
    [SerializeField] private Transform player;
    private Transform _waterObject;

    [Header("Water Rising Settings")]
    [Tooltip("Enables or disables the automatic water rising logic.")]
    [SerializeField] private bool isRising = false;

    [Tooltip("The base speed at which the water level rises.")]
    [SerializeField] private float riseSpeed = 1f;

    [Tooltip("The maximum speed when the water is far behind the player.")]
    [SerializeField] private float fasterRiseSpeed = 2f;

    [Tooltip("The rate at which the water speed changes. Higher values mean faster transitions.")]
    [SerializeField] private float acceleration = 0.5f;

    [Tooltip("The vertical distance from the player where the water starts to speed up.")]
    [SerializeField] private float maxDistanceToPlayer = 10f;

    private float _currentRiseSpeed;

    /// <summary>
    /// Gets the current y-position of the water's surface.
    /// </summary>
    public float CurrentWaterLevel => _waterObject != null ? _waterObject.position.y + 24 : -Mathf.Infinity;

    private void Start()
    {
        // Cache the water object's transform for efficiency.
        _waterObject = FindFirstObjectByType<ModernWater2D>().transform;

        // Automatically find the player if not assigned in the Inspector.
        if (!player) player = PlayerController.Instance.transform;

        OnReset();
    }

    /// <summary>
    /// Sets the water surface to a specific y-position.
    /// </summary>
    /// <param name="level">The target y-position for the water surface.</param>
    public void SetWaterLevel(float level)
    {
        if (_waterObject == null) return;
        _waterObject.position = new Vector3(_waterObject.position.x, level - 24, _waterObject.position.z);
    }

    private void Update()
    {
        // 1. Exit early if the system is disabled or essential objects are missing.
        if (!isRising || player == null || _waterObject == null)
        {
            return;
        }

        // 2. Define the target water level based on the player's position.
        float targetLevel = player.position.y;

        // 3. The water should only rise. It does not go down if the player moves down.
        if (CurrentWaterLevel < targetLevel)
        {
            // 4. Determine the target speed based on the distance to the player.
            float targetSpeed = (targetLevel - CurrentWaterLevel > maxDistanceToPlayer) ? fasterRiseSpeed : riseSpeed;

            // 5. Smoothly accelerate or decelerate the current rise speed towards the target speed.
            _currentRiseSpeed = Mathf.MoveTowards(_currentRiseSpeed, targetSpeed, acceleration * Time.deltaTime);

            // 6. Calculate the new water level for this frame using the smoothed speed.
            float newLevel = Mathf.MoveTowards(CurrentWaterLevel, targetLevel, _currentRiseSpeed * Time.deltaTime);

            // 7. Apply the newly calculated water level.
            SetWaterLevel(newLevel);
        }
    }

    #region Public Control Methods
    /// <summary>
    /// Enables the water rising system.
    /// </summary>
    public void StartRising()
    {
        isRising = true;
    }

    /// <summary>
    /// Disables the water rising system.
    /// </summary>
    public void StopRising()
    {
        isRising = false;
    }
    #endregion

    public void OnReset()
    {
        // Initialize the current speed to the base rise speed.
        _currentRiseSpeed = riseSpeed;

        // Set initial water level relative to the player.
        float targetLevel = player.position.y;
        SetWaterLevel(targetLevel - maxDistanceToPlayer / 2);
    }
}