using UnityEngine;

public class RatState : BaseState
{
    protected Rigidbody2D rb;
    protected CapsuleCollider2D col;
    protected RatController controller;

    protected Vector3 boxCastRadius;
    protected Vector3 boxCastPos;
    protected Vector3 boxPosOffset;

    protected LayerMask groundLayerMask;

    public override void OnEnterState()
    {
        controller = Owner.gameObject.GetComponent<RatController>();
        rb = controller.rb;
        col = controller.col;
        boxCastRadius = new Vector3(0.9f, 0.35f, 1f);
        boxPosOffset = new Vector3(0f, -0.1f - boxCastRadius.y / 2, 0f);
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public override void OnFixedUpdateState()
    {
        boxCastPos = controller.transform.position + boxPosOffset;
    }

    protected bool checkIsGrounded()
    {
        return Physics2D.OverlapBox(boxCastPos, boxCastRadius, 0, groundLayerMask);
    }

    protected bool checkBarelyAerial()
    {
        return Physics2D.Raycast(controller.transform.position, Vector2.down, 1f, groundLayerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCastPos, boxCastRadius);
    }
}
