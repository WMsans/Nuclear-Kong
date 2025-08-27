using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacleTypes;

    public void SpawnEnemy()
    {
        Instantiate(obstacleTypes[Random.Range(0, obstacleTypes.Count)], transform.position, Quaternion.identity);
    }
}
