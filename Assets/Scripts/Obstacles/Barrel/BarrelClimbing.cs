using UnityEngine;

public class BarrelClimbing : BarrelState
{
    [SerializeField] private float fallingSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private BarrelGrounded groundedState;
    [Header("Aerial Variables")]
    [SerializeField] private BarrelAerial aerialState;

    public override void OnEnterState()
    {
        base.OnEnterState();
        rb.linearVelocityX = 0;
        col.excludeLayers = groundLayerMask;
        controller.justTouchedLadder = false;
        controller.anim.SetTrigger("Aerial");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        col.excludeLayers = 0;
        controller.anim.ResetTrigger("Aerial");
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();

        if (!controller.ladderTileBelow)
        {
            if (checkIsGrounded())
            {
                controller.stateMachine.ChangeState(groundedState);
            }
            else
            {
                controller.stateMachine.ChangeState(aerialState);
            }
        }
        else
        {
            rb.linearVelocityY = -fallingSpeed;
        }
    }
}
