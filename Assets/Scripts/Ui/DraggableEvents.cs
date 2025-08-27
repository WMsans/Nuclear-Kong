using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Broadcasts events for a draggable UI item.
/// Attach this to the same GameObject as the Draggable component.
/// </summary>
public class DraggableEvents : MonoBehaviour, IEndDragHandler
{
    // Event that fires when a drag operation finishes over a valid slot.
    public static event Action<Draggable, Slot> OnWeaponDropped;

    // We'll store the IWeapon data here so we know which weapon this icon represents.
    public IWeapon Weapon { get; set; }

    public void OnEndDrag(PointerEventData eventData)
    {
        // When the drag ends, check if the pointer is over a Slot.
        if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent<Slot>(out var slot))
        {
            // If it is, fire the event with the draggable component and the slot.
            var draggable = GetComponent<Draggable>();
            if (draggable != null)
            {
                OnWeaponDropped?.Invoke(draggable, slot);
            }
        }
    }
}