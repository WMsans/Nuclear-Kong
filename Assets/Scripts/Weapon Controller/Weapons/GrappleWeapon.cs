using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Grapple")]
public class GrappleWeapon : ScriptableObject, IWeapon
{
    public string Name => "Grapple";
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;
    public float CoolDownWhenSynthed => 0; // Not synthesizable
    public bool CanBeSynthed => false; // Not synthesizable

    [SerializeField] private float grappleLength = 10f;
    [SerializeField] private LayerMask grabbableLayer;

    public void OnAttack(Transform attackPos, List<IWeapon> synthedWeapons)
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(InputSystemManager.Instance.CurrentFrameInput.MousePosition) - attackPos.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(attackPos.position, direction, grappleLength, grabbableLayer);
        if (hit.collider != null && hit.collider.TryGetComponent(out IGrabbable grabbable) && !grabbable.IsGrappled)
        {
            var weaponController = _owner;  
            if (weaponController != null)
            {
                var throwableWeapon = new ThrowableWeapon(grabbable, weaponController.transform);
                weaponController.CurrentSlots++;
                weaponController.AddWeaponToFront(throwableWeapon);
                grabbable.OnGrabbed(weaponController.transform);
            }
        }
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