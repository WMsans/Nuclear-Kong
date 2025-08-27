using System.Collections;
using System.Collections.Generic;
using BulletPro;
using MEC;
using UnityEngine;

public class KongRicochetState : KongActionBaseState
{
    [Header("State Settings")]
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float fireRate = 0.5f;

    [Header("Bullet Emitter")]
    [SerializeField] private BulletEmitter ricochetEmitter;

    private float _enterStateTime;

    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterStateTime = Time.time;
        Timing.RunCoroutine(PlayEmitterCoroutine());
        
        anim.SetTrigger("Jump");
    }

    private IEnumerator<float> PlayEmitterCoroutine()
    {
        yield return Timing.WaitForSeconds(0.5f);
        ricochetEmitter.Play();
    }
    public override void OnUpdateState()
    {
        if(Time.time - _enterStateTime > attackDuration) ExitAction();
    }

    public override void OnExitState()
    {
        ricochetEmitter.Stop();
    }
}