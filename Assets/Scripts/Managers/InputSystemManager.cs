using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemManager : MonoSingleton<InputSystemManager>
{
    public struct FrameInput
    {
        public Vector2 Move;
        public Vector2 MousePosition;
        public bool JumpDown;
        public bool JumpUp;
        public bool JumpHold;
        public float JumpPressTime;
        public bool AttackDown;
        public bool AttackUp;
        public bool AttackHold;
        public bool DashDown;
        public bool DashUp;
        public bool DashHold;
        public bool MenuDown;
        public bool MenuUp;
        public bool MenuHold;
        public bool LadderDown;
    }
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private float rotationInput;
    private Vector2 mousePosition;
    public FrameInput CurrentFrameInput { get; private set; } = new();
    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Aim.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        inputActions.Player.Aim.canceled += ctx => mousePosition = Vector2.zero;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleCharacterInput();
    }

    private void HandleCharacterInput()
    {
        var inputs = new FrameInput
        {
            Move = moveInput,
            MousePosition = mousePosition,
            JumpDown = inputActions.Player.Jump.WasPressedThisFrame(),
            JumpUp = inputActions.Player.Jump.WasReleasedThisFrame(),
            JumpHold = inputActions.Player.Jump.IsPressed(),
            JumpPressTime = inputActions.Player.Jump.WasPressedThisFrame() ? Time.time : CurrentFrameInput.JumpPressTime,
            AttackDown = inputActions.Player.Attack.WasPressedThisFrame(),
            AttackUp = inputActions.Player.Attack.WasReleasedThisFrame(),
            AttackHold = inputActions.Player.Attack.IsPressed(),
            DashDown = inputActions.Player.Sprint.WasPressedThisFrame(),
            DashUp = inputActions.Player.Sprint.WasReleasedThisFrame(),
            DashHold = inputActions.Player.Sprint.IsPressed(),
            MenuDown = inputActions.Player.Menu.WasPressedThisFrame(),
            MenuUp = inputActions.Player.Menu.WasReleasedThisFrame(),
            MenuHold = inputActions.Player.Menu.IsPressed(),
            LadderDown = inputActions.Player.Ladder.WasPressedThisFrame(),
        };
        CurrentFrameInput = inputs;
    }
}
