using UnityEngine;

public class KongStiffState : KongBaseState
{
    [SerializeField] private Vector2 jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float stiffTime;
    [SerializeField] private KongBaseState nextState;
    private float _enterStateTime;
    public override void OnEnterState()
    {
        base.OnEnterState();
        _enterStateTime = Time.time;
        FacePlayer();
        rb.linearVelocity = jumpForce * new Vector2(-Owner.transform.right.x, 1f);
        
        anim.SetTrigger("Idle");
    }
    private void FacePlayer()
    {
        var playerPos = PlayerController.Instance.transform.position;
        Owner.transform.rotation = Quaternion.Euler(Owner.transform.eulerAngles.x,
            playerPos.x < Owner.transform.position.x ? 180f : 0f, Owner.transform.eulerAngles.z);
    }
    public override void OnUpdateState()
    {
        if (Time.time - _enterStateTime > stiffTime)
        {
            Owner.ChangeState(nextState);
        }
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
