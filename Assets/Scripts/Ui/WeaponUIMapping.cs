using AYellowpaper.SerializedCollections;
using UnityEngine;

/// <summary>
/// Maps IWeapon ScriptableObjects to their corresponding UI prefabs for the inventory.
/// </summary>
[CreateAssetMenu(fileName = "New Weapon UI Mapping", menuName = "Custom Assets/UI/Weapon UI Mapping")]
public class WeaponUIMapping : ScriptableObject
{
    [Tooltip("Maps weapon scriptable objects to the prefabs for their UI icons.")]
    [SerializedDictionary("Weapon Asset", "UI Prefab")]
    public SerializedDictionary<ScriptableObject, GameObject> weaponToUIPrefab;
}