using MoreMountains.FeedbacksForThirdParty;
using UnityEngine;

public class BarrelAerial : BarrelState
{
    [SerializeField] private float fallingSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private BarrelGrounded groundedState;
    [Header("Climbing Variables")]
    [SerializeField] private BarrelClimbing climbingState;

    public override void OnEnterState()
    {
        base.OnEnterState();
        rb.linearVelocityX = 0;
        controller.anim.SetTrigger("Aerial");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        controller.anim.ResetTrigger("Aerial");
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();

        rb.linearVelocityY = -fallingSpeed;
        rb.linearVelocityX = 0;

        if (checkIsGrounded())
        {
            controller.stateMachine.ChangeState(groundedState);
        }

        if (controller.justTouchedLadder)
        {
            if (Random.value < 0.5f)
            {
                controller.stateMachine.ChangeState(climbingState);
            }

            controller.justTouchedLadder = false;
        }
    }
}
