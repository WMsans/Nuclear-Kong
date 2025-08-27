using UnityEngine;
using System.Linq;

public class SlimeClimbingUp : SlimeState
{
    [SerializeField] private float climbingSpeed;
    [Header("Climbing Down Variables")]
    [SerializeField] private SlimeClimbingDown climbingDownState;
    [Header("Aerial Variables")]
    [SerializeField] private SlimeAerial aerialState;
    [Header("Chasing Variables")]
    [SerializeField] private SlimeChasing chaseState;

    public override void OnEnterState()
    {
        base.OnEnterState();
        rb.linearVelocityX = 0;
        col.excludeLayers = groundLayerMask;
        controller.anim.SetTrigger("Climbing");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        col.excludeLayers = 0;
        controller.anim.ResetTrigger("Climbing");
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();

        if (!controller.ladderTileCurrent)
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
            rb.linearVelocityY = climbingSpeed;

            if (Physics2D.RaycastAll(controller.transform.position + Vector3.up * 0.4f, Vector3.up, 0.6f, controller.ladderLayer).Any(x => x.collider.CompareTag("Enemy")))
            {
                controller.stateMachine.ChangeState(climbingDownState);
            }
        }
    }
}
