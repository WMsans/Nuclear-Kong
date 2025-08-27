using UnityEngine;

public class ObstacleSpawnerBoundary : MonoBehaviour
{
    [SerializeField] private ObstacleSpawner[] spawners;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ObstacleSpawner i in spawners)
            {
                i.StartSpawning();
                i.OnReset();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ObstacleSpawner i in spawners) {
                i.StopSpawning();
            }
        }
    }
}
