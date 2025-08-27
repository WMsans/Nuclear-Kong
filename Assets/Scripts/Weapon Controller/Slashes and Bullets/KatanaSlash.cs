using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class KatanaSlash : MonoBehaviour
{
    [SerializeField] private float sustainTime;
    [SerializeField] private float damage;

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
    }
}