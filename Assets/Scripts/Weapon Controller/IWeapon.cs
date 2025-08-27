using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Defines the behavior of a weapon.
/// </summary>
public interface IWeapon
{
    float CoolDown { get; }
    float CoolDownWhenSynthed { get; }
    bool CanBeSynthed { get; }

    /// <summary>
    /// The primary attack logic, called only when this is the FIRST weapon in a cast chain.
    /// It is aware of all subsequent modifiers in the chain.
    /// </summary>
    /// <param name="shootPoint">The point from which to fire.</param>
    /// <param name="synthedWeapons">A list of subsequent weapons in the chain that are synthed with this one.</param>
    void OnAttack(Transform shootPoint, [CanBeNull] List<IWeapon> synthedWeapons);

    /// <summary>
    /// The modifier logic, called only when this weapon is NOT the first in a cast chain.
    /// It executes after the primary weapon's OnAttack.
    /// </summary>
    /// <param name="shootPoint">The point from which to fire.</param>
    void OnSynthedAttack(Transform shootPoint);
    
    void SetWeaponController(WeaponController controller);
}