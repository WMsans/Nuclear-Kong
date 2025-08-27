using UnityEngine;
using UnityEngine.Events; 

public class PlayerNormalState : PlayerState
{
    [Header("State Events")]
    public UnityEvent OnJump;
    public UnityEvent OnWallJump;
    public UnityEvent OnLand;
    public UnityEvent OnRoll;
    public UnityEvent OnStartWalking;
    public UnityEvent OnStopWalking;

    private bool _isWalking = false;

    private bool _cachedQueryStartInColliders;
    [Header("Wall Interaction Variables")]
    private bool _isAgainstWall;
    private bool _isWallSliding;
    private float _wallDirection;
    private float _timeLastTouchedWall;
    private float _lastWallDirection;
    private float _wallJumpLockoutEndTime;
    private float _lastWallJumpDirection;

    [Header("Climbing Variables")]
    [SerializeField] private PlayerClimbingState climbingState;
    [SerializeField] private PlayerRollingState rollingState; 

    public override void OnEnterState()
    {
        base.OnEnterState();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders; InitalizeJumpVariables();
        CheckJumpOnEnter();
        CheckCollisions();
    }

    private void InitalizeJumpVariables()
    {
        GatherInput();
        _endedJumpEarly = false;
        _timeJumpWasPressed = frameInput.JumpPressTime;
        _coyoteUsable = true;
        _bufferedJumpUsable = true;
        _canEndJumpEarly = false;

        _isAgainstWall = false;
        _isWallSliding = false;
        _wallDirection = 0;
        _wallJumpLockoutEndTime = -1f;
        _lastWallJumpDirection = 0;
        _timeLastTouchedWall = -Mathf.Infinity;
    }
    private void CheckJumpOnEnter()
    {
        if (_timeJumpWasPressed + stats.jumpBuffer > Time.time)
        {
            ExecuteJump();
        }
    }
    public override void OnUpdateState()
    {
        base.OnUpdateState();
        HandleInput();
        HandleAttack();
    }

    public override void OnExitState()
    {
        OnStopWalking?.Invoke();
    }

    private void HandleInput()
    {
        if (frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = Time.time;
        }

        if (controller.IsTouchingLadder && Mathf.Abs(frameInput.Move.y) > 0.1f)
        {
            controller.stateMachine.ChangeState(climbingState);
        }
        else if (frameInput.DashDown && Time.time >= controller.LastRollTime + stats.rollCooldown)
        {

            OnRoll?.Invoke();
            controller.stateMachine.ChangeState(rollingState);
        }
    }

    private void HandleAttack()
    {
        if (frameInput.AttackDown && weaponController.CoolDownTimer <= 0)
        {
            weaponController.OnUseWeapon();
        }
    }

    public override void OnFixedUpdateState()
    {
        CheckCollisions();

        HandleGravity();
        HandleDirection();

        HandleJump();
    }
    #region Collisions

    private float GetFrameLeftGrounded() => PlayerController.Instance.FrameLeftGrounded;
    private bool _grounded;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = true;

        bool groundHit = Physics2D.Raycast((Vector2)rb.position, Vector2.down, stats.grounderDistance, stats.groundLayer);
        float horizontalCheckOffset = col.bounds.extents.x - 0.02f;
        Vector2 raycastOriginLeft = new Vector2(col.bounds.center.x - horizontalCheckOffset, col.bounds.center.y);
        Vector2 raycastOriginRight = new Vector2(col.bounds.center.x + horizontalCheckOffset, col.bounds.center.y);
        bool wallHitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.left, stats.grounderDistance, stats.groundLayer);
        bool wallHitRight = Physics2D.Raycast(raycastOriginRight, Vector2.right, stats.grounderDistance, stats.groundLayer);

        _isAgainstWall = (wallHitLeft || wallHitRight) && !groundHit;

        if (_isAgainstWall)
        {
            _wallDirection = wallHitRight ? -1 : 1;
            _lastWallDirection = _wallDirection;
            _timeLastTouchedWall = Time.time;
        }
        else
        {
            _wallDirection = 0;

        }

        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            _canEndJumpEarly = true;
            _isWallSliding = false;
            _isAgainstWall = false;
            controller.CanEndJumpEarly = true;

            OnLand?.Invoke();

            if (weaponController != null)
            {
                var dashTracker = weaponController.GetComponent<KatanaDashTracker>();
                if (dashTracker != null)
                {
                    dashTracker.HasDashedInAir = false;
                }
            }
        }
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            PlayerController.Instance.UpdateFrameLeftGrounded();

            _isWallSliding = _isAgainstWall && rb.linearVelocity.y < 0;
        }

        if (_isAgainstWall && !_grounded && rb.linearVelocity.y <= 0)
        {
            _isWallSliding = true;
        }

        else if (!_isAgainstWall || _grounded || rb.linearVelocity.y > 0)
        {
            _isWallSliding = false;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    #endregion

    #region Jumping

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _canEndJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && Time.time < _timeJumpWasPressed + stats.jumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && Time.time < GetFrameLeftGrounded() + stats.coyoteTime;
    private bool CanUseWallCoyote => !_grounded  && Time.time < _timeLastTouchedWall + stats.wallJumpCoyoteTime;
    private bool CanWallJump() => (_isAgainstWall || CanUseWallCoyote) && !_grounded && (_jumpToConsume || HasBufferedJump);
    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !frameInput.JumpHold && rb.linearVelocity.y > 0 && _canEndJumpEarly && controller.CanEndJumpEarly)
        {
            _endedJumpEarly = true;
        }

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote)
        {
            ExecuteJump();
        }
        else if (CanWallJump())
        {
            ExecuteWallJump();
        }

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _isWallSliding = false;
        _canEndJumpEarly = true;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpPower);
        
        OnJump?.Invoke();
    }
    private void ExecuteWallJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _isWallSliding = false;
        _canEndJumpEarly = true;

        _lastWallJumpDirection = _lastWallDirection;
        _wallJumpLockoutEndTime = Time.time + stats.wallJumpInputLockoutDuration;

        float horizontalInput = frameInput.Move.x;
        bool hasHorizontalInput = Mathf.Abs(horizontalInput) > 0.1f;
        float horizontalForce = hasHorizontalInput ? stats.wallJumpForceHorizontalWithInput : stats.wallJumpForceHorizontalBase;
        float forceDirection = _lastWallJumpDirection;

        rb.linearVelocity = new Vector2(horizontalForce * forceDirection, stats.wallJumpForceVertical);

        OnWallJump?.Invoke();
    }
    #endregion

    #region Horizontal

    private void HandleDirection()
    {
        float horizontalInput = frameInput.Move.x;
        bool lockoutActive = Time.time < _wallJumpLockoutEndTime;

        if (lockoutActive)
        {
            if (Mathf.Approximately(Mathf.Sign(horizontalInput), _lastWallJumpDirection))
            {
                horizontalInput = 0;
            }
        }

        if (Mathf.Abs(horizontalInput) < 0.1f)
        {

            if (_isWalking)
            {
                _isWalking = false;

                OnStopWalking?.Invoke();
            }

            if (_grounded)
            {
                // rb.bodyType = RigidbodyType2D.Kinematic;
                rb.position = Physics2D.Raycast((Vector2)rb.position, Vector2.down, stats.grounderDistance,
                    stats.groundLayer).point;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // rb.bodyType = RigidbodyType2D.Dynamic;
                var deceleration = stats.airDeceleration;
                rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, 0, deceleration * Time.fixedDeltaTime), rb.linearVelocity.y);
            }
        }
        else
        {

            if (!_isWalking && _grounded)
            {
                _isWalking = true;

                OnStartWalking?.Invoke();
            }

            rb.bodyType = RigidbodyType2D.Dynamic;

            if (!_grounded && Mathf.Approximately(Mathf.Sign(horizontalInput), Mathf.Sign(rb.linearVelocity.x)) && Mathf.Abs(rb.linearVelocity.x) > stats.maxSpeed)
            {

            }
            else
            {
                float targetXVelocity = horizontalInput * stats.maxSpeed;
                float currentAcceleration = stats.acceleration;
                rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, targetXVelocity, currentAcceleration * Time.fixedDeltaTime), rb.linearVelocity.y);
            }

            Owner.transform.rotation = Quaternion.Euler(Owner.transform.eulerAngles.x, horizontalInput < 0 ? 180f : 0f, Owner.transform.eulerAngles.z);
        }
    }

    #endregion

    #region Gravity

    private void HandleGravity()
    {
        if (_isWallSliding)
        {
            if (rb.linearVelocity.y < -stats.wallSlideSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -stats.wallSlideSpeed);
            }
            else if (rb.linearVelocity.y > -stats.wallSlideSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.MoveTowards(rb.linearVelocity.y, -stats.maxFallSpeed, stats.fallAcceleration * .2f * Time.fixedDeltaTime));
            }
        }
        else if (_grounded)
        {
            if (rb.linearVelocity.y <= 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -stats.groundingForce);
            }
        }
        else
        {
            var inAirGravity = stats.fallAcceleration;
            if (_endedJumpEarly && rb.linearVelocity.y > 0 && _canEndJumpEarly && controller.CanEndJumpEarly)
            {
                inAirGravity *= stats.jumpEndEarlyGravityModifier;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.MoveTowards(rb.linearVelocity.y, -stats.maxFallSpeed, inAirGravity * Time.fixedDeltaTime));
        }
    }

    #endregion
}