using UnityEngine;

public class BarrelGrounded : BarrelState
{
    private bool isLeft;
    [SerializeField] float rollSpeed;
    [Header("Aerial Variables")]
    [SerializeField] private BarrelAerial aerialState;
    [Header("Climbing Variables")]
    [SerializeField] private BarrelClimbing climbingState;
    private float raycastXLength = 0.1f;
    private float raycastXWallLength = 0.5f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        RaycastHit2D cast = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayerMask);
        float ang = Vector2.Angle(cast.normal, Vector2.up);
        if (ang < 10f && ang > -10f)
        {
            isLeft = controller.player.transform.position.x < controller.transform.position.x;
        }
        else
        {
            isLeft = cast.normal.x < 0;
        }

        controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
        if (controller.CheckIsWallForward())
        {
            isLeft = !isLeft;
        }

        rb.linearVelocityY = 0;
        controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
        controller.anim.SetTrigger("Grounded");
        controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
        controller.slopeChecker = Physics2D.Raycast(transform.position - new Vector3(0f, col.radius, 0f), new Vector2(isLeft ? 0.5f : -0.5f, 0f), 0.15f, groundLayerMask);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        controller.anim.ResetTrigger("Grounded");
    }


    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        rb.linearVelocityX = rollSpeed * (isLeft ? -1 : 1);
        controller.currentlyGrounded = checkIsGrounded();
        controller.slopeChecker = Physics2D.Raycast(transform.position - new Vector3(0f, col.radius, 0f), new Vector2(isLeft ? 0.5f : -0.5f, 0f), 0.15f, groundLayerMask);

        if (controller.justTouchedLadder && controller.ladderTileBelow)
        {
            if (Random.value < 0.3f)
            {
                controller.stateMachine.ChangeState(climbingState);
            }

            controller.justTouchedLadder = false;
        }

        if (controller.wallForward)
        {
            controller.hurt();
        }

        if (!checkIsGrounded() && !checkBarelyAerial())
        {
            controller.stateMachine.ChangeState(aerialState);
        }
        else if ((!checkIsGrounded() && checkBarelyAerial()) || (checkIsGrounded() && !controller.slopeChecker))
        {
            rb.linearVelocityY = -rollSpeed;
        }
        else
        {
            rb.linearVelocityY = 0;
        }
    }
}
