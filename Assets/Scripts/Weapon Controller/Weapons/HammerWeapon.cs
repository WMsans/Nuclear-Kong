using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Hammer")]
public class HammerWeapon : ScriptableObject, IWeapon
{
    public string Name => "Hammer";
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;
    [SerializeField] private float coolDownWhenSynthed;
    public float CoolDownWhenSynthed => coolDownWhenSynthed;
    public bool CanBeSynthed => true;
    [SerializeField] private GameObject slashPrefab;
    private Animator _animator;
    public void OnAttack(Transform shootPoint, List<IWeapon> synthedWeapons)
    {
        Instantiate(slashPrefab, shootPoint);
        _animator.SetTrigger("HammerSwing");
    }

    public void OnSynthedAttack(Transform shootPoint)
    {
        Instantiate(slashPrefab, shootPoint);
    }

    public void SetWeaponController(WeaponController controller)
    {
        if(!controller) return;
        _animator = PlayerController.Instance.animator;
    }
}
