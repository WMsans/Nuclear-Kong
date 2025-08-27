using System;
using System.Collections;
using System.Collections.Generic;
using Destructible2D;
using MEC;
using UnityEngine;

public class HammerSlash : MonoBehaviour
{
    [SerializeField] private float sustainTime;
    [SerializeField] private float damage;
    private float _lastFractureTime;
    private void Start()
    {
        Timing.RunCoroutine(SelfDestroyCoroutine(), gameObject);
    }

    private IEnumerator<float> SelfDestroyCoroutine()
    {
        yield return Timing.WaitForSeconds(sustainTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IHarmable>(out var harmable))
        {
            harmable.hurt();
            // TODO: Add damage
        }

        var fracturer = other.GetComponentInParent<FracturerHandler>();
        if (fracturer && Time.time - _lastFractureTime > 2f)
        {
            fracturer.Fracture();
            _lastFractureTime = Time.time;
        }

        if (other.TryGetComponent<ButtonBehaviour>(out var buttonBehaviour))
        {
            buttonBehaviour.PressButton();
        }
    }
}
