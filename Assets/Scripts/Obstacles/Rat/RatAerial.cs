using UnityEngine;

public class RatAerial : RatState
{
    [SerializeField] private float fallingSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private RatGrounded groundedState;
    [Header("Chasing Variables")]
    [SerializeField] private RatChasing chaseState;

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
    }
}
