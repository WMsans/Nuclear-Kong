using BulletPro;
using UnityEngine;

public abstract class KongBaseState : BaseState
{
    [SerializeField] protected LayerMask groundLayer = 64;
    protected Rigidbody2D rb;
    protected Animator anim;

    public override void OnEnterState()
    {
        rb = Owner.GetComponent<Rigidbody2D>();
        anim = Owner.GetComponentInChildren<Animator>();
    }
}
