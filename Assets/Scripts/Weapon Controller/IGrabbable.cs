using UnityEngine;

public interface IGrabbable
{
    bool IsGrappled { get; }
    void OnGrabbed(Transform holder);
    void OnThrow(Vector2 force);
    GameObject GetGameObject();
}