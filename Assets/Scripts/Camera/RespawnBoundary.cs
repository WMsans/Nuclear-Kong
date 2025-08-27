using System;
using System.Linq;
using UnityEngine;

public class RespawnBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerPos = (Vector2)other.transform.position;
            var hitDown = Physics2D.Raycast(playerPos + Vector2.up, Vector2.up, Mathf.Infinity, 64);
            var hit = Physics2D.Raycast(playerPos, Vector2.down, Mathf.Infinity, 64);
            if(hit.distance < 2.5f) RespawnManager.Instance.SetSpawnPoint(hit.point);
            else RespawnManager.Instance.SetSpawnPoint(hitDown.point + 0.6f * Vector2.up);

            var resetables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IResetable>();
            foreach(var x in resetables) x?.OnReset();
        }
    }
}
