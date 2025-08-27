using UnityEngine.EventSystems;

public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler
{
    void OnDroppedInSlot(Slot slot);
    void OnLeaveSlot(Slot slot);
    void OnReturnToSlot();
}