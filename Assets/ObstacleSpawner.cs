using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;   // prefab obstacle
    public Transform player;            // referensi ke player
    public float spawnDistance = 20f;   // seberapa jauh obstacle muncul di depan player
    public float minY = -0.5f;          // posisi Y obstacle
    public float minDelay = 1.5f;       // waktu minimum antar spawn
    public float maxDelay = 3f;         // waktu maksimum antar spawn

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            ScheduleNextSpawn();
        }
    }

    void SpawnObstacle()
    {
        Vector3 spawnPos = new Vector3(player.position.x + spawnDistance, minY, 0);
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minDelay, maxDelay);
    }
}
