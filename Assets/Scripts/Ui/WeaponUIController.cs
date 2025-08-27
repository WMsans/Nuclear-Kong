using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the weapon UI, creating slots and handling drag-and-drop reordering via events.
/// </summary>
public class WeaponUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private WeaponUIMapping weaponUIMapping;
    [SerializeField] private GameObject defaultWeaponIconPrefab;

    [Header("UI Settings")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;

    private readonly List<Slot> uiSlots = new List<Slot>();

    private void OnEnable()
    {
        // Listen for events from the systems
        if (weaponController != null)
        {
            weaponController.OnWeaponsChanged += UpdateWeaponIcons;
            weaponController.OnSlotsChanged += UpdateSlots;
        }
        DraggableEvents.OnWeaponDropped += HandleWeaponDrop;
    }

    private void OnDisable()
    {
        // Stop listening to events
        if (weaponController != null)
        {
            weaponController.OnWeaponsChanged -= UpdateWeaponIcons;
            weaponController.OnSlotsChanged -= UpdateSlots;
        }
        DraggableEvents.OnWeaponDropped -= HandleWeaponDrop;
    }

    private void Start()
    {
        UpdateSlots();
        UpdateWeaponIcons();
    }

    /// <summary>
    /// Re-creates the UI slots based on the number of available slots in the WeaponController.
    /// </summary>
    private void UpdateSlots()
    {
        // Clear old slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }
        uiSlots.Clear();

        if (weaponController == null) return;

        // Create new slots
        for (var i = 0; i < weaponController.CurrentSlots; i++)
        {
            var slotGO = Instantiate(slotPrefab, slotContainer);
            var slot = slotGO.GetComponent<Slot>();
            if (slot != null)
            {
                uiSlots.Add(slot);
            }
        }
        UpdateWeaponIcons(); // Refresh icons after rebuilding slots
    }

    /// <summary>
    /// Refreshes all weapon icons in the UI to match the current state of the WeaponController.
    /// </summary>
    private void UpdateWeaponIcons()
    {
        if (weaponController == null || weaponUIMapping == null) return;

        // First, clear all existing icons from the slots
        foreach (var slot in uiSlots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Now, repopulate the slots with the correct weapon icons in the correct order
        for (var i = 0; i < weaponController.CurrentWeapons.Count; i++)
        {
            if (i >= uiSlots.Count) break; // Safety check

            var weapon = weaponController.CurrentWeapons[i];
            GameObject iconGO;
            if (weapon is ScriptableObject weaponSO && weaponUIMapping.weaponToUIPrefab.TryGetValue(weaponSO, out var prefab))
            {
                iconGO = Instantiate(prefab, uiSlots[i].transform);
            }
            else
            {
                iconGO = Instantiate(defaultWeaponIconPrefab, uiSlots[i].transform);
            }
            var draggableEvents = iconGO.GetComponent<DraggableEvents>();
            if (draggableEvents != null)
            {
                draggableEvents.Weapon = weapon;
            }
        }
    }

    /// <summary>
    /// Handles the reordering logic when a weapon drop event is received.
    /// </summary>
    private void HandleWeaponDrop(Draggable draggedItem, Slot newSlot)
    {
        if (draggedItem == null || newSlot == null || weaponController == null) return;

        var oldSlot = draggedItem.OriginalParent.GetComponent<Slot>();
        var fromIndex = uiSlots.IndexOf(oldSlot);
        var toIndex = uiSlots.IndexOf(newSlot);

        // If the indices are valid, tell the weapon controller to reorder its list.
        // The UI will then update automatically via the OnWeaponsChanged event.
        if (fromIndex != -1 && toIndex != -1 && fromIndex != toIndex)
        {
            weaponController.ReorderWeapon(fromIndex, toIndex);
        }
    }
}