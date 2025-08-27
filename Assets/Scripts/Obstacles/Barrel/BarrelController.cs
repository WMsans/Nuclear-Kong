using UnityEngine;
using System.Linq;
using System.Collections;

public class BarrelController : MonoBehaviour, IHarmable, IGrabbable, IResetable
{
    public Rigidbody2D rb;
    public CircleCollider2D col;
    public PlayerController player;
    [Header("States")]
    public StateMachineRunner stateMachine;
    [Header("Information")]
    public bool groundForward;
    public bool wallForward;
    public Vector3 raycastGroundDirection;
    public Vector3 raycastWallDirection;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Animator anim;
    public bool slopeChecker;
    public bool currentlyGrounded;
    public GameObject explosion;

    [Header("Ladder Information")]
    public LayerMask ladderLayer;
    public bool justTouchedLadder;
    public bool ladderTileBelow;
    public bool ladderTileCurrent;

    [Header("Grabbable Information")]
    private Transform _originalParent;
    public bool IsGrappled { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _originalParent = transform.parent;
    }


    public void OnGrabbed(Transform holder)
    {
        transform.SetParent(holder);
        stateMachine.enabled = false;
        transform.localPosition = Vector2.right * 2; // Position in front of player
        IsGrappled = true;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void OnThrow(Vector2 force)
    {
        transform.SetParent(_originalParent);
        stateMachine.enabled = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        IsGrappled = false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerHarmable>(out var harmable))
        {
            harmable.OnDead();
            hurt();
        }

        if (collision.collider.TryGetComponent<IHarmable>(out var aharmable))
        {
            aharmable.hurt();
            hurt();
        }
    }

    void FixedUpdate()
    {
        groundForward = Physics2D.Raycast(transform.position, raycastGroundDirection, 1f, groundLayer);
        RaycastHit2D cast = Physics2D.Raycast(transform.position + new Vector3(0f, col.radius / 2, 0f), raycastWallDirection, 1f, groundLayer);
        if (!cast)
        {
            wallForward = false;
        }
        else
        {
            float castDistance = cast.distance;
            if (Vector2.Angle(cast.normal, Vector2.up) < 80)
            {
                castDistance -= 0.6f;
            }
            wallForward = castDistance < 0.5f;
        }



        bool previousLadder = ladderTileCurrent || ladderTileBelow;

        ladderTileCurrent = Physics2D.OverlapCircleAll(transform.position, 0.1f, ladderLayer).Any(x => x.CompareTag("Ladder"));
        ladderTileBelow = Physics2D.OverlapCircleAll(transform.position + new Vector3(0f, -1f, 0f), 0.1f, ladderLayer).Any(x => x.CompareTag("Ladder"));

        justTouchedLadder = (ladderTileBelow || ladderTileCurrent) && !previousLadder;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PointManager.Instance.AddPoints(20);
        }
    }


    public void hurt()
    {
        SoundManagerObject.Instance.PlayBarrelDestroy();
        SoundManagerObject.Instance.PlayBarrelSpawn();
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public bool CheckIsWallForward()
    {
        return Physics2D.Raycast(transform.position + new Vector3(0f, col.radius / 2, 0f), raycastWallDirection, 1f, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, -1f, 0f), 0.1f);
    }

    public void OnReset()
    {
        Destroy(this.gameObject);
    }
}
