using System.Collections;
using UnityEngine;

public class RatController : MonoBehaviour, IHarmable, IResetable
{
    public Rigidbody2D rb;
    public CapsuleCollider2D col;
    public Animator anim;
    [Header("States")]
    public StateMachineRunner stateMachine;
    [Header("Information")]
    public bool groundForward;
    public bool wallForward;
    public bool upwardsSlope;
    public Vector3 raycastGroundDirection;
    public Vector3 raycastWallDirection;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public SpriteRenderer sprite;
    public bool chasing = false;
    public float chaseRadius;
    public bool dead;

    void Start()
    {
        SoundManagerObject.Instance.PlayRatSpawn();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerHarmable>(out var harmable) && !dead)
        {
            harmable.OnDead();
        }
    }

    public void FixedUpdate()
    {
        groundForward = Physics2D.Raycast(transform.position, raycastGroundDirection, 2f, groundLayer);
        wallForward = Physics2D.Raycast(transform.position + new Vector3(0f, col.size.y / 3f, 0f), raycastWallDirection, 0.7f, groundLayer);

        upwardsSlope = Physics2D.Raycast(transform.position + new Vector3(0f, col.size.y / 3f, 0f), raycastWallDirection, 1.4f, groundLayer) && !Physics2D.Raycast(transform.position, new Vector3(1f, 1f, 0f), 1f, groundLayer);

        //chasing = Physics2D.OverlapCircle(transform.position, chaseRadius, playerLayer);
    }

    public void hurt()
    {
        StartCoroutine(die());
    }

    public IEnumerator die()
    {
        anim.SetTrigger("Death");
        PointManager.Instance.AddPoints(150);
        stateMachine.enabled = false;
        dead = true;
        SoundManagerObject.Instance.PlayRatDeath();
        col.enabled = false;

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    public void OnReset()
    {
        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, raycastGroundDirection);
        Gizmos.DrawRay(transform.position + new Vector3(0f, col.size.y / 3f, 0f), raycastWallDirection);
    }
}
