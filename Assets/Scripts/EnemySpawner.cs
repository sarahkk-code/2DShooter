
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRatePerMinute = 8f;

    private float spawnTimer = 0f;

    void Start()
    {
        // Use the spawn rate selected in Preferences.
        spawnRatePerMinute = PlayerPrefs.GetFloat("SpawnRate", 8f);

        // Spawn the first enemy immediately.
        SpawnEnemy();
    }

    void Update()
    {
        if (enemyPrefab == null || spawnRatePerMinute <= 0f)
            return;

        spawnTimer += Time.deltaTime;

        float spawnInterval = 60f / spawnRatePerMinute;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer -= spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
            return;

        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0f);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
