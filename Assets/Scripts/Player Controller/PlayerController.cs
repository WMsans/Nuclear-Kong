using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoSingleton<PlayerController>
{
    public Rigidbody2D rb;
    public Animator animator;
    public CapsuleCollider2D col;
    public PlayerStats stats;
    public WeaponController weaponController;
    [Header("States")]
    public StateMachineRunner stateMachine;
    public float FrameLeftGrounded { get; private set; }
    public Vector2 OriginalColliderSize { get; private set; } // To store the original collider size
    public float LastRollTime { get; private set; } = -Mathf.Infinity; // Track the time of the last roll


    public bool CanEndJumpEarly { get; set; } = true; 

    public bool IsTouchingLadder => Physics2D.OverlapCapsule(rb.position + col.offset + .1f * InputSystemManager.Instance.CurrentFrameInput.Move, col.size - .1f * Vector2.one, col.direction, transform.eulerAngles.z, stats.ladderLayer);

    public void UpdateFrameLeftGrounded() => FrameLeftGrounded = Time.time;

    public void DisableEarlyJumpEndUntilGrounded()
    {
        CanEndJumpEarly = false;
    }

    public void SetLastRollTime()
    {
        LastRollTime = Time.time;
    }


    private void Start()
    {
        FrameLeftGrounded = Time.time;
        OriginalColliderSize = col.size; // Store the original collider size on start
        menuMusic.Instance.startSound();
    }

    private void OnDrawGizmos()
    {
        if (!stats) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rb.position + (stats.grounderDistance + col.size.y / 2 - col.offset.y) * Vector2.down, .05f);
    }

    public bool GetGrounded()
    {
        return Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, stats.grounderDistance, stats.groundLayer);
    }

    // New method to change the collider height
    public void SetColliderHeight(float height)
    {
        Vector2 newSize = new Vector2(col.size.x, height);
        col.size = newSize;
        // Adjust offset to keep the collider's bottom at the same position
        col.offset = new Vector2(col.offset.x, height / 2);
    }
}