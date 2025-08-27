using System.Collections;
using System.Collections.Generic;
using BulletPro;
using DG.Tweening;
using UnityEngine;

public class KongBulletHellState : KongActionBaseState
{
    [SerializeField] private Vector2 centerPoint;
    [SerializeField] private float hellTime;
    [SerializeField] private BulletEmitter emitter;
    private float _enterStateTime;
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterStateTime = Time.time;
        rb.DOMove(centerPoint, .5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            emitter.Play();
        });
        
        anim.SetTrigger("Jump");
    }

    public override void OnUpdateState()
    {
        if(Time.time - _enterStateTime > hellTime) Owner.ChangeState(LastState);
    }

    public override void OnExitState()
    {
        emitter.Stop();
    }
}
