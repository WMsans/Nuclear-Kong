using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BarrelState : BaseState
{
    protected Rigidbody2D rb;
    protected CircleCollider2D col;
    protected BarrelController controller;

    protected Vector3 sphereCastRadius;
    protected Vector3 sphereCastPos;
    protected Vector3 spherePosOffset;
    protected LayerMask groundLayerMask;

    public override void OnEnterState()
    {
        controller = Owner.gameObject.GetComponent<BarrelController>();
        rb = controller.rb;
        col = controller.col;
        controller.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sphereCastRadius = new Vector3(0.9f, 0.5f, 1f) * col.radius * 2f;
        spherePosOffset = new Vector3(0f, -sphereCastRadius.y / 1.9f, 0f);
        sphereCastPos = controller.gameObject.transform.position + spherePosOffset;
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public override void OnFixedUpdateState()
    {
        sphereCastPos = controller.gameObject.transform.position + spherePosOffset;
    }

    protected bool checkIsGrounded()
    {
        return Physics2D.OverlapBox(sphereCastPos, sphereCastRadius, 0, groundLayerMask);
    }

    protected bool checkBarelyAerial()
    {
        return Physics2D.Raycast(controller.transform.position, Vector2.down, 1f, groundLayerMask);
    }
}
