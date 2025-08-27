using System.Linq;
using UnityEngine;

public class PlayerClimbingState : PlayerState
{
    [SerializeField] private PlayerNormalState normalState;
    public override void OnEnterState()
    {
        base.OnEnterState();
        int playerLayer = controller.gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & stats.groundLayer) != 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, i, true);
            }
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        int playerLayer = controller.gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & stats.groundLayer) != 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, i, false);
            }
        }
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        if (frameInput.JumpDown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpPower);
            controller.stateMachine.ChangeState(normalState);
            return;
        }

        if (!controller.IsTouchingLadder)
        {
            if (CheckTouchingGround(rb.position + col.offset))
            {
                // Player is at the top of the ladder and on the ground, so we stop them from climbing further.
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // Player is no longer touching the ladder and not on the ground, so they fall.
                controller.stateMachine.ChangeState(normalState);
            }
        }
    }

    private bool CheckTouchingGround(Vector2 pos) =>
        Physics2D.OverlapCapsule(pos, col.size, col.direction, 0, stats.groundLayer);

    private bool CheckTouchingLadder(Vector2 pos) =>
        Physics2D.OverlapCapsule(
            pos + .1f * InputSystemManager.Instance.CurrentFrameInput.Move,
            col.size - .1f * Vector2.one, col.direction, transform.eulerAngles.z, stats.ladderLayer);

    public override void OnFixedUpdateState()
    {
        var dis = new Vector2(frameInput.Move.x * stats.climbSpeed, frameInput.Move.y * stats.climbSpeed) * Time.fixedDeltaTime;
        if (CheckTouchingGround((Vector2)col.bounds.center + dis) &&
            !CheckTouchingLadder((Vector2)col.bounds.center + dis))
            rb.linearVelocity = Vector2.zero;
        else 
            rb.linearVelocity = new Vector2(frameInput.Move.x * stats.climbSpeed, frameInput.Move.y * stats.climbSpeed);
    }
}