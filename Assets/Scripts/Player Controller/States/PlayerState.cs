using UnityEngine;

public abstract class PlayerState : BaseState
{
    [SerializeField] protected PlayerStats stats;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected CapsuleCollider2D col;
    protected PlayerController controller;
    protected WeaponController weaponController;
    protected InputSystemManager.FrameInput frameInput;

    public override void OnEnterState()
    {
        controller = PlayerController.Instance;
        rb = controller.rb;
        animator = controller.animator;
        col = controller.col as CapsuleCollider2D;
        weaponController = controller.weaponController;
        GatherInput();
    }
    public override void OnUpdateState()
    {
        GatherInput();
    }
    protected void GatherInput()
    {
        frameInput = InputSystemManager.Instance.CurrentFrameInput;
    }
}
