using System.Linq;
using UnityEngine;

public class SlimeGrounded : SlimeState
{
    private bool isLeft;
    [SerializeField] float walkSpeed;
    [Header("Climbing Down Variables")]
    [SerializeField] private SlimeClimbingDown climbingDownState;
    [Header("Climbing Up Variables")]
    [SerializeField] private SlimeClimbingUp climbingUpState;
    [Header("Chasing Variables")]
    [SerializeField] private SlimeChasing chaseState;
    [Header("Aerial Variables")]
    [SerializeField] private SlimeAerial aerialState;
    private float raycastXLength = 0.15f;
    private float raycastXWallLength = 0.3f;
    private bool enemyForward;

    public override void OnEnterState()
    {
        base.OnEnterState();
        if (Physics2D.Raycast(transform.position, new Vector2(isLeft ? -0.5f : 0.5f, -0.5f)))
        {
            isLeft = Random.value < 0.5 ? true : false;
            controller.sprite.flipX = isLeft;
        }
        controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
        controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
        controller.anim.SetTrigger("Walking");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        controller.anim.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        controller.anim.ResetTrigger("Walking");
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        rb.linearVelocityX = walkSpeed * (isLeft ? -1 : 1);

        if (controller.justTouchedLadder)
        {
            if (Random.value < 0.5f)
            {
                if (controller.ladderTileBelow)
                {
                    controller.stateMachine.ChangeState(climbingDownState);
                }
                else if (controller.ladderTileCurrent)
                {
                    controller.stateMachine.ChangeState(climbingUpState);
                }
            }

            controller.justTouchedLadder = false;
        }
        else
        {
            enemyForward = Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.4f : Vector3.right * 0.4f), isLeft ? Vector3.left : Vector3.right, 0.6f, controller.ladderLayer).Any(x => x.collider.CompareTag("Enemy"));
            enemyForward = enemyForward || Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.4f : Vector3.right * 0.4f), isLeft ? Vector3.left : Vector3.right + Vector3.down, 0.6f, controller.ladderLayer).Any(x => x.collider.CompareTag("Enemy"));
            enemyForward = enemyForward || Physics2D.RaycastAll(controller.transform.position + (isLeft ? Vector3.left * 0.4f : Vector3.right * 0.4f), isLeft ? Vector3.left : Vector3.right + Vector3.up, 0.6f, controller.ladderLayer).Any(x => x.collider.CompareTag("Enemy"));

            if (!controller.groundForward || controller.wallForward || enemyForward)
            {
                isLeft = !isLeft;
                controller.sprite.flipX = isLeft;
                controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
                controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
            }
        }

        if (controller.chasing)
        {
            controller.stateMachine.ChangeState(chaseState);
        }

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
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f * (isLeft ? -1 : 1)));
        }
        else if (!checkIsGrounded() && checkBarelyAerial())
        {
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -45f * (isLeft ? -1 : 1)));
            controller.anim.gameObject.transform.localPosition = new Vector3(-0.11f * (isLeft ? -1 : 1), -0.11f, 0f);
        }
        else
        {
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            controller.anim.gameObject.transform.localPosition = new Vector3(0f, -0.042f, 0f);
        }
    }
}
