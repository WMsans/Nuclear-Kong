using System;
using UnityEngine;

public class MovingPlatformTriggerZone : MonoBehaviour
{
    public event Action OnPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}