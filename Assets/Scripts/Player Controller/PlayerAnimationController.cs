using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private static readonly int Climbing = Animator.StringToHash("Climbing");
    private static readonly int Hor = Animator.StringToHash("Hor");
    private static readonly int Ver = Animator.StringToHash("Ver");
    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerController _controller;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _controller = PlayerController.Instance;
    }

    private void Update()
    {
        _animator.SetBool(Climbing, _controller.stateMachine.CurrentState is PlayerClimbingState);
        _animator.SetFloat(Hor, Mathf.Abs(_rb.linearVelocity.x));
        _animator.SetFloat(Ver, Mathf.Abs(_rb.linearVelocity.y));
    }
}
