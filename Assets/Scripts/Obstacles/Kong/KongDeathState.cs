using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class KongDeathState : KongBaseState
{
    [SerializeField] private float gravity;
    public UnityEvent onDeath;
    public override void OnEnterState()
    {
        base.OnEnterState();
        anim.SetTrigger("Idle");
        Owner.transform.DORotate(new(0f, 0f, 90f), 0.25f).OnComplete(onDeath.Invoke);
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
