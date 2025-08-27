using DG.Tweening;
using UnityEngine;

public class SlimeChasing : SlimeState
{
    private bool isLeft;
    [SerializeField] float walkSpeed;
    [Header("Grounded Variables")]
    [SerializeField] private SlimeGrounded groundedState;
    [Header("Aerial Variables")]
    [SerializeField] private SlimeAerial aerialState;
    [Header("Climbing Up Variables")]
    [SerializeField] private SlimeClimbingUp climbingUpState;
    [Header("Climbing Down Variables")]
    [SerializeField] private SlimeClimbingDown climbingDownState;
    private Transform playerLocation;
    private Transform myLocation;
    private float raycastXLength = 0.15f;
    private float raycastXWallLength = 0.3f;

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
        controller.anim.SetTrigger("Walking");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        controller.anim.ResetTrigger("Walking");
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        isLeft = myLocation.position.x > playerLocation.position.x;
        controller.sprite.flipX = isLeft;
        controller.raycastGroundDirection.x = isLeft ? -raycastXLength : raycastXLength;
        controller.raycastWallDirection.x = isLeft ? -raycastXWallLength : raycastXWallLength;
        rb.linearVelocityX = walkSpeed * (isLeft ? -1 : 1);

        if (Mathf.Abs(playerLocation.position.x - myLocation.position.x) < 0.05f)
        {
            rb.linearVelocityX = 0;
        }

        if (controller.justTouchedLadder)
        {
            if (playerLocation.position.y > myLocation.position.y + 0.6f && controller.ladderTileCurrent)
            {
                controller.stateMachine.ChangeState(climbingUpState);
            }
            else if (playerLocation.position.y < myLocation.position.y && controller.ladderTileBelow)
            {
                controller.stateMachine.ChangeState(climbingDownState);
            }

            controller.justTouchedLadder = false;
        }

        if (!controller.chasing)
        {
            controller.stateMachine.ChangeState(groundedState);
        }

        if (!checkIsGrounded() && !checkBarelyAerial())
        {
            controller.stateMachine.ChangeState(aerialState);
        }
        else if (!checkIsGrounded() && checkBarelyAerial())
        {
            rb.linearVelocityY = -walkSpeed;
        }
        else
        {
            rb.linearVelocityY = 0;
        }

        if (controller.upwardsSlope)
        {
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f * (isLeft ? -1 : 1)));
        }
        else if (!checkIsGrounded() && checkBarelyAerial())
        {
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -45f * (isLeft ? -1 : 1)));
            controller.anim.gameObject.transform.localPosition = new Vector3(-0.11f * (isLeft ? -1 : 1), -0.11f, 0f);
        }
        else
        {
            controller.anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            controller.anim.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
