using UnityEngine;
using System.Linq;

public class RatGrounded : RatState
{
    private bool isLeft;
    [SerializeField] float walkSpeed;
    [Header("Aerial Variables")]
    [SerializeField] private RatAerial aerialState;
    private float raycastXLength = 0.15f;
    private float raycastXWallLength = 0.7f;
    private bool enemyForward;
    private LayerMask ladderLayer;

    public override void OnEnterState()
    {
        ladderLayer = LayerMask.GetMask("Default");
        base.OnEnterState();
        if (Physics2D.Raycast(transform.position, new Vector2(isLeft ? -0.5f : 0.5f, -0.5f)))
        {
            isLeft = Random.value < 0.5 ? true : false;
        }
        rb.linearVelocityY = 0;
        controller.sprite.flipX = !isLeft;
        controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
        controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        rb.linearVelocityX = walkSpeed * (isLeft ? -1 : 1);

        enemyForward = Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.55f : Vector3.right * 0.55f), isLeft ? Vector3.left : Vector3.right, 0.6f, ladderLayer).Any(x => x.collider.CompareTag("Enemy"));
        enemyForward = enemyForward || Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.55f : Vector3.right * 0.55f), isLeft ? Vector3.left : Vector3.right + Vector3.down, 0.6f, ladderLayer).Any(x => x.collider.CompareTag("Enemy"));
        enemyForward = enemyForward || Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.55f : Vector3.right * 0.55f), isLeft ? Vector3.left : Vector3.right + Vector3.up, 0.6f, ladderLayer).Any(x => x.collider.CompareTag("Enemy"));

        if (!controller.groundForward || controller.wallForward || enemyForward)
        {
            controller.sprite.flipX = isLeft;
            isLeft = !isLeft;
            controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
            controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
        }

        /*if (controller.chasing)
        {
            controller.stateMachine.ChangeState(chaseState);
        }*/

        if (!checkIsGrounded() && !checkBarelyAerial())
        {
            controller.stateMachine.ChangeState(aerialState);
        }
        else if (!checkIsGrounded() && checkBarelyAerial())
        {
            rb.linearVelocityY = -walkSpeed;
        }
        else
        {
            rb.linearVelocityY = 0;
        }
        

        if (controller.upwardsSlope)
        {
            Debug.Log("Upwards slope");
            controller.sprite.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f * (isLeft ? -1 : 1)));
        }
        else if (!checkIsGrounded() && checkBarelyAerial())
        {
            controller.sprite.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -45f * (isLeft ? -1 : 1)));
            controller.sprite.gameObject.transform.localPosition = new Vector3(-0.11f * (isLeft ? -1 : 1), -0.11f, 0f);
        }
        else
        {
            controller.sprite.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            controller.sprite.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
