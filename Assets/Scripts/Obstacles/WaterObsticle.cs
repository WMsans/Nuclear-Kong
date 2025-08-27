using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObsticle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHarmable>(out var harmable))
        {
            harmable.OnDead();
        }

        if (other.TryGetComponent<IHarmable>(out var aharmable))
        {
            aharmable.hurt();
        }
    }
}
