using UnityEngine;

public class RatChasing : RatState
{
    private bool isLeft;
    [SerializeField] float walkSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private RatGrounded groundedState;
    [Header("Aerial Variables")]
    [SerializeField] private RatAerial aerialState;
    private Transform playerLocation;
    private Transform myLocation;

    public void Start()
    {
        playerLocation = GameObject.Find("Player").transform;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        myLocation = controller.transform;
        rb.linearVelocityY = 0;
        isLeft = myLocation.position.x > playerLocation.position.x;
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        isLeft = myLocation.position.x > playerLocation.position.x;
        rb.linearVelocityX = walkSpeed * (isLeft ? -1 : 1);

        if (Mathf.Abs(playerLocation.position.x - myLocation.position.x) < 0.05f)
        {
            rb.linearVelocityX = 0;
        }

        if (!checkIsGrounded())
        {
            controller.stateMachine.ChangeState(aerialState);
        }

        if (!controller.chasing)
        {
            controller.stateMachine.ChangeState(groundedState);
        }
    }
}
