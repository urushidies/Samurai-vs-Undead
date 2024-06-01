using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public WaveData waveData;
    public Transform spawnPoint;

    private int currentWaveIndex = 0;
    public bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        if (waveData == null)
        {
            Debug.LogError("WaveData is not assigned!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is not assigned!");
            return;
        }

        StartNextWave();
    }

    public void Update()
    {
        if (isSpawning)
            return;

        if (currentWaveIndex < waveData.waves.Length)
        {
            if (AreEnemiesSpawned() && AreAllEnemiesDead())
            {
                isSpawning = true;
                Invoke(nameof(StartNextWave), 2f); // Wait for 2 seconds before starting the next wave
            }
        }
        else
        {
            if (AreAllEnemiesDead())
            {
                Debug.Log("You won!");
            }
        }
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waveData.waves.Length)
        {
            Debug.Log("No more waves to spawn.");
            return;
        }

        Debug.Log($"Starting wave {currentWaveIndex + 1}");
        isSpawning = false;
        StartCoroutine(SpawnWaveWithDelay(waveData.waves[currentWaveIndex]));
        currentWaveIndex++;
    }

    private bool AreEnemiesSpawned()
    {
        // Assume enemies are spawned if this method is called
        return true;
    }

    public bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator SpawnWaveWithDelay(EnemyWave wave)
    {
        Debug.Log($"Spawning wave with {wave.enemies.Length} enemies");

        foreach (EnemySpawn enemySpawn in wave.enemies)
        {
            if (enemySpawn.enemyPrefab == null)
            {
                Debug.LogError("Enemy prefab is not assigned in the WaveData!");
                continue;
            }

            Vector3 spawnPosition = spawnPoint.position + new Vector3(enemySpawn.spawnPosition.x, enemySpawn.spawnPosition.y, 0);
            Debug.Log($"Spawning enemy at position {spawnPosition} with delay {enemySpawn.spawnDelay}");

            GameObject enemy = Instantiate(enemySpawn.enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            // Wait for the specified delay before spawning the next enemy
            yield return new WaitForSeconds(enemySpawn.spawnDelay);
        }

        // Set isSpawning to false to allow starting the next wave
        isSpawning = false;
    }
}
