using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongCoolDownState : KongBaseState
{
    [SerializeField] private List<KongActionBaseState> actionStates;
    [SerializeField] private float coolDown;
    [SerializeField] private float gravity;

    [Header("State Transition")] 
    [SerializeField] private KongHpCounter hpCounter;
    [SerializeField] private KongBaseState nextState;
    private float _enterStateTime;
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterStateTime = Time.time;
        
        anim.SetTrigger("Idle");
    }

    public override void OnUpdateState()
    {
        HandleActions();
        HandleNextState();
    }

    private void HandleNextState()
    {
        if(hpCounter.CurrentHp <= 0f)
        {
            hpCounter.ResetHp();
            Owner.ChangeState(nextState);
        }
    }

    private void HandleActions()
    {
        if(!CanAttack()) return;
        var nextActionIdx = Random.Range(0, actionStates.Count);
        var nextAction = actionStates[nextActionIdx];
        nextAction.LastState = this;
        Owner.ChangeState(nextAction);
    }

    private bool CanAttack() => Time.time - _enterStateTime >= coolDown;
    
    public override void OnFixedUpdateState()
    {
        HandleGravity();
    }

    private void HandleGravity()
    {
        rb.linearVelocity += Vector2.down * (gravity * Time.fixedDeltaTime);
    }
}
