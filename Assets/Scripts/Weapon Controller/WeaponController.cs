using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponController : MonoBehaviour, IResetable
{
    public event Action OnWeaponsChanged;
    public event Action OnSlotsChanged;
    [Tooltip("The initial set of weapons to equip.")]
    [CanBeNull] [SerializeField] private List<ScriptableObject> initialWeapons = new();

    [Tooltip("The maximum number of weapons that can be equipped.")]
    [SerializeField] private int initialSlots;
    
    [Tooltip("The transform representing where projectiles or effects should originate.")]
    [SerializeField] private Transform shootPoint;

    // The master list of currently equipped weapons, in order.
    public List<IWeapon> CurrentWeapons { get; private set; } = new();
    private int _currentSlots;
    public int CurrentSlots
    {
        get => _currentSlots;
        set
        {
            if (_currentSlots != value)
            {
                _currentSlots = value;
                OnSlotsChanged?.Invoke(); // Fire event
            }
        }
    }
    
    // The timer tracking the cooldown between casts.
    public float CoolDownTimer { get; private set; }

    // An index to track our position in the CurrentWeapons list.
    private int _nextWeaponIndex;

    private void Start()
    {
        CurrentSlots = initialSlots;
        _nextWeaponIndex = 0;
        
        if (initialWeapons != null)
        {
            foreach (var weaponSO in initialWeapons)
            {
                if (weaponSO is IWeapon weapon)
                {
                    AddWeapon(weapon);
                }
            }
        }
        if (CurrentWeapons.Count > 0)
        {
            OnWeaponsChanged?.Invoke();
        }
    }

    private void Update()
    {
        if (CoolDownTimer > 0)
        {
            CoolDownTimer -= Time.deltaTime;
        }
        else
        {
            CoolDownTimer = 0;
        }

        RemoveNullWeapons();
    }

    private void RemoveNullWeapons()
    {
        var weaponsToGo = CurrentWeapons.ToArray();
        foreach (var x in weaponsToGo)
        {
            try
            {
                if (x is ThrowableWeapon a && !a.grabbable.GetGameObject())
                {
                    RemoveWeapon(x);
                }
            }
            catch (MissingReferenceException)
            {
                RemoveWeapon(x);
            }
        }
    }
    
    public void OnUseWeapon()
    {
        if (CoolDownTimer > 0 || CurrentWeapons.Count == 0) return;

        if (_nextWeaponIndex >= CurrentWeapons.Count)
        {
            _nextWeaponIndex = 0; // Reload cycle
        }
        
        // --- Build the Attack Chain ---
        var primaryWeapon = CurrentWeapons[_nextWeaponIndex];
        var synthedWeapons = new List<IWeapon>();
        var totalCooldown = primaryWeapon.CoolDown;
        
        _nextWeaponIndex++;

        while (_nextWeaponIndex < CurrentWeapons.Count && CurrentWeapons[_nextWeaponIndex].CanBeSynthed)
        {
            var modifierWeapon = CurrentWeapons[_nextWeaponIndex];
            synthedWeapons.Add(modifierWeapon);
            totalCooldown += modifierWeapon.CoolDownWhenSynthed;
            _nextWeaponIndex++;
        }
        
        // --- Execute the Attack ---
        CoolDownTimer = totalCooldown;

        // 1. The primary weapon fires first using OnAttack.
        //    It's aware of the whole chain, which is useful for "multicast" type spells.
        primaryWeapon.OnAttack(shootPoint, synthedWeapons.Count > 0 ? synthedWeapons : null);

        // 2. Then, each synthesized weapon executes its own modifier logic in order using OnSynthedAttack.
        //    This is useful for effects that apply sequentially, like "Drill" then "Bounce".
        foreach (var modifierWeapon in synthedWeapons)
        {
            modifierWeapon.OnSynthedAttack(shootPoint);
        }
    }
    
    public void AddWeapon(IWeapon newWeapon)
    {
        if (CurrentWeapons.Count >= CurrentSlots) return;

        CurrentWeapons.Add(newWeapon);
        newWeapon.SetWeaponController(this);
        OnWeaponsChanged?.Invoke(); // Fire event
    }

    public void AddWeaponToFront(IWeapon newWeapon)
    {
        if (CurrentWeapons.Count >= CurrentSlots) return;

        CurrentWeapons.Insert(0, newWeapon);
        newWeapon.SetWeaponController(this);
        _nextWeaponIndex++;
        OnWeaponsChanged?.Invoke(); // Fire event
    }

    public void RemoveWeapon(IWeapon weaponToRemove)
    {
        int removedIndex = CurrentWeapons.IndexOf(weaponToRemove);
        if (removedIndex == -1) return;

        CurrentWeapons.RemoveAt(removedIndex);
        weaponToRemove.SetWeaponController(null);
        
        if (removedIndex < _nextWeaponIndex)
        {
            _nextWeaponIndex--;
        }
        _nextWeaponIndex = Mathf.Clamp(_nextWeaponIndex, 0, CurrentWeapons.Count);
        OnWeaponsChanged?.Invoke();
    }

    public void RemoveAllWeapons()
        {
            // Iterate through a copy of the list to perform cleanup on each weapon.
            // This avoids issues with modifying a collection while iterating over it.
            foreach (var weapon in CurrentWeapons.ToList())
            {
                weapon.SetWeaponController(null);
            }
            
            // Clear the master list of all its elements.
            CurrentWeapons.Clear();
            
            // Reset the index for the weapon firing cycle.
            _nextWeaponIndex = 0;
            
            // Fire the event to notify UI and other systems that the weapons have changed.
            OnWeaponsChanged?.Invoke();
        }
    public void ReorderWeapon(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= CurrentWeapons.Count || toIndex < 0 || fromIndex == toIndex)
        {
            return;
        }
        // If dropping on a slot that has an item, we need to swap, not just move.
        // If dropping on an empty slot, the logic is simpler.
        // This handles both cases by removing and inserting.
        if (toIndex >= CurrentWeapons.Count)
        {
            toIndex = CurrentWeapons.Count-1;
        }
        
        IWeapon weapon = CurrentWeapons[fromIndex];
        CurrentWeapons.RemoveAt(fromIndex);
        CurrentWeapons.Insert(toIndex, weapon);

        OnWeaponsChanged?.Invoke(); // Fire event to trigger UI refresh
    }

    public void OnReset()
    {
        RemoveAllWeapons();
    }
}