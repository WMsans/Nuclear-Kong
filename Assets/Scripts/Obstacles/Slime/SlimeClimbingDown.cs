using UnityEngine;
using System.Linq;

public class SlimeClimbingDown : SlimeState
{
    [SerializeField] private float fallingSpeed;
    [Header("Climbing Up Variables")]
    [SerializeField] private SlimeClimbingUp climbingUpState;
    [Header("Grounded Variables")]
    [SerializeField] private SlimeGrounded groundedState;
    [Header("Aerial Variables")]
    [SerializeField] private SlimeAerial aerialState;
    [Header("Chasing Variables")]
    [SerializeField] private SlimeChasing chaseState;

    public override void OnEnterState()
    {
        base.OnEnterState();
        rb.linearVelocityX = 0;
        col.excludeLayers = groundLayerMask;
        controller.justTouchedLadder = false;
        controller.anim.SetTrigger("Climbing");
        controller.sprite.flipY = true;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        col.excludeLayers = 0;
        controller.anim.ResetTrigger("Climbing");
        controller.sprite.flipY = false;
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();

        if (!controller.ladderTileBelow)
        {
            if (controller.chasing)
            {
                controller.stateMachine.ChangeState(chaseState);
            }
            else
            {
                controller.stateMachine.ChangeState(aerialState);
            }
        }
        else
        {
            rb.linearVelocityY = -fallingSpeed;

            if (Physics2D.RaycastAll(controller.transform.position + Vector3.down * 0.4f, Vector3.down, 0.6f, controller.ladderLayer).Any(x => x.collider.CompareTag("Enemy")))
            {
                controller.stateMachine.ChangeState(climbingUpState);
            }
        }
    }
}
