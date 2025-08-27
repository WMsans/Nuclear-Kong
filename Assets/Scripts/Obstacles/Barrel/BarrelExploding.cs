using System.Collections;
using UnityEngine;

public class BarrelExploding : MonoBehaviour, IResetable
{
    public void OnReset()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerHarmable>(out var harmable))
        {
            harmable.OnDead();
        }

        if (collision.collider.TryGetComponent<IHarmable>(out var aharmable))
        {
            aharmable.hurt();
        }
    }

    
}
