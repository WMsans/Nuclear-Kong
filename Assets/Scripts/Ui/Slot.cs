using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation Parameters")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationDuration = 0.2f;

    // We'll store the RectTransform to avoid getting it repeatedly.
    private RectTransform rectTransform;
    // Store the original size (width and height) instead of scale.
    private Vector2 originalSize;

    private void Awake()
    {
        // Cache the RectTransform component.
        rectTransform = GetComponent<RectTransform>();
        // Store the original sizeDelta.
        originalSize = rectTransform.sizeDelta;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Animate back to original size.
        rectTransform.DOSizeDelta(originalSize, animationDuration).SetUpdate(true);

        // Get the item being dragged
        Draggable draggedItem = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggedItem == null) return;

        // Get the item that's already in this slot (if any)
        Draggable itemInSlot = GetComponentInChildren<Draggable>();

        if (itemInSlot != null && itemInSlot != draggedItem)
        {
            // Find the original slot of the item we are dragging
            Slot originalSlot = draggedItem.OriginalParent.GetComponent<Slot>();
            if (originalSlot == null) return;

            // 1. Move the item that was in this slot to the original slot
            itemInSlot.SetAndAnimateToSlot(originalSlot);

            // 2. Move the dragged item to this slot
            draggedItem.SetAndAnimateToSlot(this);
        }
        else
        {
            draggedItem.SetAndAnimateToSlot(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only animate if the user is dragging something.
        if (eventData.pointerDrag != null)
        {
            // Animate the sizeDelta instead of localScale.
            rectTransform.DOSizeDelta(originalSize * hoverScale, animationDuration).SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only animate if the user is dragging something.
        if (eventData.pointerDrag != null)
        {
            // Animate back to the original size.
            rectTransform.DOSizeDelta(originalSize, animationDuration).SetUpdate(true);

            // The rest of your logic can remain the same.
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggable != null)
            {
                // This interface call is hypothetical based on your original code.
                // draggable.OnLeaveSlot(this);
            }
        }
    }
}