using UnityEngine;

public class BarrelIdle : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerHarmable>(out var pharmable))
        {
            pharmable.OnDead();
        }

        if (collision.collider.TryGetComponent<IHarmable>(out var harmable))
        {
            harmable.hurt();
        }
    }
}
