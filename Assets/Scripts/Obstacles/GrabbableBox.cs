using UnityEngine;

public class GrabbableBox : MonoBehaviour, IGrabbable
{
    private Rigidbody2D _rb;
    private Transform _originalParent;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalParent = transform.parent;
    }

    public bool IsGrappled { get; private set; }

    public void OnGrabbed(Transform holder)
    {
        transform.SetParent(holder);
        transform.localPosition = Vector2.right * 2; // Position in front of player
        IsGrappled = true;
        if(_rb != null) _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void OnThrow(Vector2 force)
    {
        transform.SetParent(_originalParent);
        if(_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.AddForce(force, ForceMode2D.Impulse);
        }

        IsGrappled = false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}