using System;
using Destructible2D;
using UnityEngine;

[RequireComponent(typeof(D2dFracturer))]
public class FracturerHandler : MonoBehaviour, IResetable
{
    [SerializeField] private bool initialCanBeFractured;
    private D2dFracturer _fracturer;
    public GameObject _unfracturedClone { get; set; }
    public bool CanBeFractured { get; private set; }= false;
    private void Awake()
    {
        _fracturer = GetComponent<D2dFracturer>();
        CanBeFractured = initialCanBeFractured;
    }

    public void SetCanBeFractured(bool canbe = true)
    {
        CanBeFractured = canbe;
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ForceFracture()
    {
        SetCanBeFractured(true);
        Fracture();
    }

    public bool Fracture()
    {
        if (!CanBeFractured) return false;
        if (gameObject.name.Contains("Clone")) return false;
        var a = Instantiate(gameObject, transform.parent);
        _unfracturedClone = a;
        _unfracturedClone.SetActive(false);
        _fracturer.TryFracture();
        CanBeFractured = false;
        return true;
    }

    public void OnReset()
    {
        if (name.Contains("Clone"))
        {
            Destroy(gameObject);
            return;
        }
        if(_unfracturedClone)
        {
            Debug.Log(name);
            _unfracturedClone.name = name;
            _unfracturedClone.SetActive(true);
            Destroy(gameObject);
            return;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
}
