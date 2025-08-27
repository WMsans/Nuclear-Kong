using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// A block that spawns a random weapon pickup when hit by the player from below.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class QuestionBlock : MonoBehaviour
{
    [Header("Spawning")]
    [Tooltip("A list of weapon pickup prefabs to choose from.")]
    [SerializeField] private List<GameObject> weaponPickupPrefabs;

    [Tooltip("The point from which the weapon will spawn.")]
    [SerializeField] private Transform spawnPoint;

    [Header("Block State")]
    [Tooltip("The sprite to display after the block has been hit.")]
    [SerializeField] private Sprite emptyBlockSprite;

    private bool isHit = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ensure the top part of the block is not a trigger
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit || !collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Check if the player hit the block from below
        if (collision.contacts[0].normal.y > 0.5f)
        {
            Hit();
        }
    }

    private void Hit()
    {
        isHit = true;

        // Change to the empty block sprite
        if (spriteRenderer != null && emptyBlockSprite != null)
        {
            spriteRenderer.sprite = emptyBlockSprite;
        }

        // Spawn a random weapon
        if (weaponPickupPrefabs.Count > 0)
        {
            int index = Random.Range(0, weaponPickupPrefabs.Count);
            GameObject weaponToSpawn = weaponPickupPrefabs[index];
            GameObject spawnedWeapon = Instantiate(weaponToSpawn, spawnPoint.position, Quaternion.identity);

            // Animate the weapon popping up
            spawnedWeapon.transform.DOJump(spawnPoint.position + new Vector3(0, 1.5f, 0), 1f, 1, 0.5f).SetEase(Ease.OutQuad);
        }

        // Animate the block itself
        transform.DOShakePosition(0.2f, 0.1f);
    }
}