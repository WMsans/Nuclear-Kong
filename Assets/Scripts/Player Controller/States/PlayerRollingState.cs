using UnityEngine;

public class PlayerRollingState : PlayerState
{
    [SerializeField] private PlayerNormalState normalState;
    private float rollEndTime;
    private Vector2 rollDirection;

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        // Set the cooldown
        controller.SetLastRollTime();

        // Set roll direction based on player's facing direction
        rollDirection = new Vector2(controller.transform.right.x, 0);

        // Apply roll speed
        rb.linearVelocity = new Vector2(rollDirection.x * stats.rollSpeed, 0);

        // Set the collider to the rolling height
        controller.SetColliderHeight(stats.rollColliderHeight);

        // Set the time when the roll will end
        rollEndTime = Time.time + stats.rollDuration;
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();

        // Transition back to normal state after roll duration
        if (Time.time >= rollEndTime)
        {
            controller.stateMachine.ChangeState(normalState);
        }
    }

    public override void OnFixedUpdateState()
    {
        // HandleGravity();
    }

    private void HandleGravity()
    {
        var inAirGravity = stats.fallAcceleration;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.MoveTowards(rb.linearVelocity.y, -stats.maxFallSpeed, inAirGravity * Time.fixedDeltaTime));
    }

    public override void OnExitState()
    {
        base.OnExitState();

        // Restore original collider height
        controller.SetColliderHeight(controller.OriginalColliderSize.y);
    }
}