using UnityEngine;

/// <summary>
/// A component to track the aerial dash state for the Katana weapon.
/// This should be attached to the same GameObject as the WeaponController.
/// </summary>
public class KatanaDashTracker : MonoBehaviour
{
    /// <summary>
    /// Gets or sets a value indicating whether the player has performed a dash in the air.
    /// This is reset when the player lands.
    /// </summary>
    public bool HasDashedInAir { get; set; }
}