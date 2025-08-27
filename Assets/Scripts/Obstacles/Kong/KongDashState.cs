using System.Collections;
using System.Collections.Generic;
using BulletPro;
using DG.Tweening;
using UnityEngine;

public class KongDashState : KongActionBaseState
{
    [SerializeField] private float precast;
    [SerializeField] private float precastDistance;
    [SerializeField] private float forwardDistance;
    [SerializeField] private BulletEmitter emitter;
    private float _enterActionTime;
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterActionTime = Time.time;
        FacePlayer();
        DOTween.To(() => rb.position, x => rb.position = x, rb.position - (Vector2)Owner.transform.right * precastDistance, precast)
            .SetEase(Ease.InQuad)
            .OnComplete(HandleDashForward);
        
        anim.SetTrigger("Walk");
    }

    private void HandleDashForward()
    {
        DOTween.To(() => rb.position, x => rb.position = x, rb.position + forwardDistance * (Vector2)Owner.transform.right,
            .35f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            emitter.Play();
            ExitAction();
        });
    }
}
