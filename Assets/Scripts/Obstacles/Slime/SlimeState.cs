using UnityEngine;

public abstract class SlimeState : BaseState
{
    protected Rigidbody2D rb;
    protected CircleCollider2D col;
    protected SlimeController controller;

    protected Vector3 boxCastRadius;
    protected Vector3 boxCastPos;
    protected Vector3 boxPosOffset;

    protected LayerMask groundLayerMask;

    public override void OnEnterState()
    {
        controller = Owner.gameObject.GetComponent<SlimeController>();
        rb = controller.rb;
        col = controller.col;
        boxCastRadius = new Vector3(0.5f, 0.25f, 1f);
        boxPosOffset = new Vector3(0f, -boxCastRadius.y / 4, 0f);
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public override void OnFixedUpdateState()
    {
        boxCastPos = controller.gameObject.transform.position + boxPosOffset;
    }

    protected bool checkIsGrounded()
    {
        return Physics2D.OverlapBox(boxCastPos, boxCastRadius, 0, groundLayerMask);
    }

    protected bool checkBarelyAerial()
    {
        return Physics2D.Raycast(controller.transform.position, Vector2.down, 1.3f, groundLayerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCastPos, boxCastRadius);
    }
}
