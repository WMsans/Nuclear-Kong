using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Weapons/Katana")]
public class KatanaWeapon : ScriptableObject, IWeapon
{
    public string Name => "Katana";
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;
    public float CoolDownWhenSynthed => 0.2f; 
    public bool CanBeSynthed => true; 

    [SerializeField] private GameObject slashPrefab;
    [FormerlySerializedAs("dashDistance")] [SerializeField] private Vector2 dashForce ;
    [SerializeField] private Vector2 smallDashForce;

    private WeaponController _owner;
    private Animator _animator;

    public void OnAttack(Transform shootPoint, List<IWeapon> synthedWeapons)
    {
        if (_owner == null) return;

        // Get or add the dash tracker component on the player
        var dashTracker = _owner.GetComponent<KatanaDashTracker>();
        if (dashTracker == null)
        {
            Debug.LogWarning("KatanaDashTracker not found on player, adding it automatically.");
            dashTracker = _owner.gameObject.AddComponent<KatanaDashTracker>();
        }

        // Instantiate the slash prefab
        if (slashPrefab != null)
        {
            Instantiate(slashPrefab, shootPoint);
        }

        // Player dash
        var playerRb = _owner.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Determine dash direction based on player orientation
            Vector2 dashDirection =
                ((Vector2)(Camera.main.ScreenToWorldPoint(InputSystemManager.Instance.CurrentFrameInput.MousePosition) -
                  shootPoint.position)).normalized;
            
            playerRb.transform.rotation = Quaternion.Euler(playerRb.transform.eulerAngles.x, dashDirection.x < .1f ? 180f : 0f, playerRb.transform.eulerAngles.z);
            
            if (dashTracker.HasDashedInAir)
            {
                playerRb.linearVelocity = dashDirection * smallDashForce;
            }
            else
            {
                playerRb.linearVelocity = dashDirection * dashForce;
                if (!PlayerController.Instance.GetGrounded())
                {
                    dashTracker.HasDashedInAir = true;
                    PlayerController.Instance.DisableEarlyJumpEndUntilGrounded(); // ADDED: Disable early jump end after air dash
                }
            }
        }
        _animator?.SetTrigger("PipeSwing");
    }

    public void OnSynthedAttack(Transform shootPoint)
    {
        if (_owner == null) return;

        // Get or add the dash tracker component on the player
        var dashTracker = _owner.GetComponent<KatanaDashTracker>();
        if (dashTracker == null)
        {
            Debug.LogWarning("KatanaDashTracker not found on player, adding it automatically.");
            dashTracker = _owner.gameObject.AddComponent<KatanaDashTracker>();
        }

        // Player dash
        var playerRb = _owner.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Determine dash direction based on player orientation
            Vector2 dashDirection =
                ((Vector2)(Camera.main.ScreenToWorldPoint(InputSystemManager.Instance.CurrentFrameInput.MousePosition) -
                           shootPoint.position)).normalized;
            
            playerRb.transform.rotation = Quaternion.Euler(playerRb.transform.eulerAngles.x, dashDirection.x < .1f ? 180f : 0f, playerRb.transform.eulerAngles.z);
            
            if (dashTracker.HasDashedInAir)
            {
                playerRb.linearVelocity = dashDirection * smallDashForce;
            }
            else
            {
                playerRb.linearVelocity = dashDirection * dashForce;
                if (!PlayerController.Instance.GetGrounded())
                {
                    dashTracker.HasDashedInAir = true;
                    PlayerController.Instance.DisableEarlyJumpEndUntilGrounded(); // ADDED: Disable early jump end after air dash
                }
            }
        }
    }

    public void SetWeaponController(WeaponController controller)
    {
        if(!controller) return;
        _owner = controller;
        _animator = PlayerController.Instance.animator;
    }
}