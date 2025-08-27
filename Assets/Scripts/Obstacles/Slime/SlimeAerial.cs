using UnityEngine;

public class SlimeAerial : SlimeState
{
    [SerializeField] private float fallingSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private SlimeGrounded groundedState;
    [Header("Climbing Down Variables")]
    [SerializeField] private SlimeClimbingDown climbingDownState;
    [Header("Chasing Variables")]
    [SerializeField] private SlimeChasing chaseState;

    public override void OnEnterState()
    {
        base.OnEnterState();
        rb.linearVelocityX = 0;
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();

        rb.linearVelocityY = -fallingSpeed;

        if (checkIsGrounded())
        {
            if (controller.chasing)
            {
                controller.stateMachine.ChangeState(chaseState);
            }
            else
            {
                controller.stateMachine.ChangeState(groundedState);
            }
        }

        if (controller.justTouchedLadder)
        {
            if (Random.value < 0.5f)
            {
                controller.stateMachine.ChangeState(climbingDownState);
            }

            controller.justTouchedLadder = false;
        }
    }
}
