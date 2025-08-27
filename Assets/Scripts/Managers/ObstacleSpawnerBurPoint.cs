using System;
using UnityEngine;

public class ObstacleSpawnerBurPoint : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Transform spawnPoint;
    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > cooldown)
        {
            Instantiate(obstacle, spawnPoint.position, spawnPoint.rotation);
            lastSpawnTime = Time.time;
        }
    }
}
