using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleSpawner : MonoBehaviour, IResetable
{
    [SerializeField][FormerlySerializedAs("Min Spawn Interval")] private float maxSpawnIntervalLow;
    [SerializeField][FormerlySerializedAs("Max Spawn Interval")] private float maxSpawnIntervalHigh;
    private float spawnInterval;
    [SerializeField] private float spawnAttempts;
    private float maxSpawnAttemptsPer = 20;
    private int children;
    private bool spawning;

    void Start()
    {
        children = gameObject.transform.childCount;
        spawnInterval = Random.Range(maxSpawnIntervalLow, maxSpawnIntervalHigh);
        spawning = false;
    }

    public void StartSpawning()
    {
        spawning = true;
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    void Update()
    {
        if (spawning)
        {
            spawnInterval -= Time.deltaTime;

            if (spawnInterval <= 0)
            {
                List<int> clearedPoints = new List<int>();
                for (int x = 0; x < spawnAttempts; x++)
                {
                    for (int i = 0; i < maxSpawnAttemptsPer; i++)
                    {
                        int randomChildIndex = Random.Range(0, children);
                        if (!clearedPoints.Contains(randomChildIndex))
                        {
                            SpawnEnemyAtIndex(randomChildIndex);
                            clearedPoints.Add(randomChildIndex);
                            break;
                        }
                    }
                }
                spawnInterval = Random.Range(maxSpawnIntervalLow, maxSpawnIntervalHigh);
            }
        }
    }

    public void SpawnEnemyAtIndex(int index)
    {
        EnemySpawnPoint spawnPoint = gameObject.transform.GetChild(index).GetComponent<EnemySpawnPoint>();
        spawnPoint.SpawnEnemy();
    }

    public void SpawnAllEnemiesOnce()
    {
        for (int i = 0; i < children; i++)
        {
            SpawnEnemyAtIndex(i);
        }
    }

    public void OnReset()
    {
        spawnInterval = Random.Range(maxSpawnIntervalLow, maxSpawnIntervalHigh);
        if (spawning)
        {
            SpawnAllEnemiesOnce();
        }
    }
}
