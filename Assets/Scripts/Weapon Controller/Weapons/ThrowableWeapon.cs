using System;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : IWeapon
{
    public readonly IGrabbable grabbable;
    private readonly Transform player;

    public string Name => $"Thrown {grabbable.GetGameObject().name}";
    public float CoolDown => 0.5f;
    public float CoolDownWhenSynthed => 0;
    public bool CanBeSynthed => false;

    public ThrowableWeapon(IGrabbable grabbable, Transform player)
    {
        this.grabbable = grabbable;
        this.player = player;
    }

    public void OnAttack(Transform shootPoint, List<IWeapon> synthedWeapons)
    {
        Vector2 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position).normalized;
        try
        {
            if (grabbable.GetGameObject()) grabbable?.OnThrow(throwDirection * 15f);
        } // Adjust force as needed
        catch(NullReferenceException)
        {
            
        }

        if (player.TryGetComponent<Rigidbody2D>(out var playerRb))
        {
            playerRb.AddForce(-throwDirection * 30f, ForceMode2D.Impulse); // Recoil force
        }

        if (!_owner) return;
        _owner.CurrentSlots--;
        _owner.RemoveWeapon(this);
    }

    public void OnSynthedAttack(Transform shootPoint)
    {
    }

    private WeaponController _owner;
    public void SetWeaponController(WeaponController controller)
    {
        if(!controller) return;
        _owner = controller;
    }
}