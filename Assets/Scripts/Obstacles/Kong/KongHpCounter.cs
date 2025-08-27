using System.Collections;
using System.Collections.Generic;
using BulletPro;
using DG.Tweening;
using UnityEngine;

public class KongHpCounter : MonoBehaviour, IHarmable, IResetable
{
    [SerializeField] private float initialHp;
    [SerializeField] private KongBaseState initialState;
    [SerializeField] private SpriteRenderer sprRenderer;
    public float CurrentHp { get; private set; }
    public void hurt()
    {
        CurrentHp -= 1f;
        
        SoundManagerObject.Instance.PlayBarrelDestroy();

        DOTween.To(() => sprRenderer.color, x => sprRenderer.color = x, Color.red, .1f).OnComplete(() =>
        {
            DOTween.To(() => sprRenderer.color, x => sprRenderer.color = x, Color.white, .1f);
        });
    }

    public void ResetHp() => CurrentHp = initialHp;

    private void Awake()
    {
        CurrentHp = initialHp;
    }

    private void Update()
    {
        // Debug.Log(CurrentHp);
    }

    public void OnReset()
    {
        ResetHp();
        var stateMachine = GetComponent<StateMachineRunner>();
        var emitters = FindObjectsByType<BulletEmitter>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var x in emitters)
        {
            x.Stop();
        }
        stateMachine.ChangeState(initialState);
        
        foreach (var bullet in BulletPoolManager.instance.pool)
        {
            bullet.Die();
        }
    }
}
