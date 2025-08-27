using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections;

public class SlimeController : MonoBehaviour, IHarmable, IResetable
{
    public Rigidbody2D rb;
    public CircleCollider2D col;
    public Animator anim;
    public SpriteRenderer sprite;
    [Header("States")]
    public StateMachineRunner stateMachine;
    [Header("Information")]
    public bool groundForward;
    public bool wallForward;
    public bool upwardsSlope;
    public bool enemyForward;
    public Vector3 raycastGroundDirection;
    public Vector3 raycastWallDirection;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public bool chasing = false;
    public float chaseRadius;
    public bool dead;

    [Header("Ladder Information")]
    public LayerMask ladderLayer;
    public bool justTouchedLadder;
    public bool ladderTileBelow;
    public bool ladderTileCurrent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerHarmable>(out var harmable) && !dead)
        {
            harmable.OnDead();
        }
    }

    public void FixedUpdate()
    {
        groundForward = Physics2D.Raycast(transform.position, raycastGroundDirection, 1f, groundLayer);
        wallForward = Physics2D.Raycast(transform.position + new Vector3(0f, col.radius / 4, 0f), raycastWallDirection, 0.3f, groundLayer);
        upwardsSlope = Physics2D.Raycast(transform.position + new Vector3(0f, col.radius / 4, 0f), raycastWallDirection, 0.6f, groundLayer) && !Physics2D.Raycast(transform.position, raycastWallDirection * 2 + new Vector3(0f, 0.6f, 0f), 1f, groundLayer);

        bool previousLadder = ladderTileCurrent || ladderTileBelow;

        ladderTileCurrent = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.25f, 0.1f, ladderLayer).Any(x => x.CompareTag("Ladder"));
        ladderTileBelow = Physics2D.OverlapCircleAll(transform.position + new Vector3(0f, -1.25f, 0f), 0.1f, ladderLayer).Any(x => x.CompareTag("Ladder"));

        justTouchedLadder = (ladderTileBelow || ladderTileCurrent) && !previousLadder;

        // remove chasing
        // chasing = Physics2D.OverlapCircle(transform.position, chaseRadius, playerLayer);
    }

    public void hurt()
    {
        StartCoroutine(die());
    }

    public void OnReset()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator die()
    {
        PointManager.Instance.AddPoints(200);
        anim.SetTrigger("Death");
        stateMachine.enabled = false;
        dead = true;
        anim.ResetTrigger("Walking");
        anim.ResetTrigger("Climbing");
        rb.linearVelocity = Vector2.zero;
        col.enabled = false;
        
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, -0.25f, 0f), 0.1f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, -1.25f, 0f), 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, raycastGroundDirection);
        Gizmos.DrawRay(transform.position + new Vector3(0f, col.radius / 4, 0f), raycastWallDirection);
        Gizmos.DrawRay(transform.position, new Vector3(1f, 1f, 0f) * 0.7f);
    }


}
