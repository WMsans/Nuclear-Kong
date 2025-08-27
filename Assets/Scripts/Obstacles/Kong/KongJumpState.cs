using System.Collections;
using System.Collections.Generic;
using BulletPro;
using MEC;
using UnityEngine;

public class KongJumpState : KongActionBaseState
{
    [SerializeField] private Vector2 jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private BulletEmitter emitter;
    private float _enterStateTime;
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterStateTime = Time.time;
        FacePlayer();
        rb.linearVelocity = jumpForce * new Vector2(Owner.transform.right.x, 1f);
        
        anim.SetTrigger("Jump");
    }

    public override void OnFixedUpdateState()
    {
        HandleGravity();
        HandleGrounding();
    }

    private void HandleGrounding()
    {
        var groundHit = Physics2D.Raycast(rb.position, Vector2.down, .05f, groundLayer);
        if(!groundHit || Time.time - _enterStateTime < .1f) return;
        emitter.Play();
        ExitAction();
    }

    private void HandleGravity()
    {
        rb.linearVelocity += Vector2.down * (gravity * Time.fixedDeltaTime);
    }
}
