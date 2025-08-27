using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongAngryState: KongBaseState
{
    [SerializeField] private List<KongActionBaseState> actionStates;
    [SerializeField] private float gravity;

    [Header("State Transition")] 
    [SerializeField] private KongHpCounter hpCounter;
    [SerializeField] private KongBaseState nextState;
    private bool _attacked;
    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log(_attacked);
        if(_attacked) Owner.ChangeState(nextState);
        else HandleActions();
    }

    private void HandleActions()
    {
        var nextActionIdx = Random.Range(0, actionStates.Count);
        var nextAction = actionStates[nextActionIdx];
        nextAction.LastState = this;
        _attacked = true;
        Owner.ChangeState(nextAction);
    }
    
    public override void OnFixedUpdateState()
    {
        HandleGravity();
    }

    private void HandleGravity()
    {
        rb.linearVelocity += Vector2.down * (gravity * Time.fixedDeltaTime);
    }
}
