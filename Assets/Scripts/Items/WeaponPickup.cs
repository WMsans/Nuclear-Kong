using UnityEngine;

/// <summary>
/// Handles the logic for a weapon pickup. When the player enters the trigger,
/// it adds the specified weapon to the player's WeaponController and then
/// destroys itself.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour, IResetable
{
    [Tooltip("The weapon to be granted to the player. This should be a ScriptableObject that implements the IWeapon interface.")]
    [SerializeField] private ScriptableObject weaponScriptableObject;

    private IWeapon weapon;

    private void Awake()
    {
        // Ensure the collider is a trigger
        var col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"The collider on {gameObject.name} is not set to be a trigger. It has been set automatically.", this);
            col.isTrigger = true;
        }

        // Validate that the provided ScriptableObject is a weapon
        weapon = weaponScriptableObject as IWeapon;
        if (weapon == null)
        {
            Debug.LogError($"The assigned ScriptableObject on {gameObject.name} does not implement the IWeapon interface.", this);
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (!other.CompareTag("Player")) return;

        // Attempt to get the WeaponController from the player
        var weaponController = other.GetComponent<WeaponController>();
        if (weaponController == null)
        {
            Debug.LogError("Player does not have a WeaponController component.", other);
            return;
        }

        // Add the weapon to the player's controller
        weaponController.AddWeapon(weapon);

        // Destroy the pickup object after it has been collected
        gameObject.SetActive(false);
    }

    public void OnReset()
    {
        gameObject.SetActive(true);
    }
}