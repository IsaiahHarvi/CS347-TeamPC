using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves; 
    private int currentWaveIndex = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            for (int i = 0; i < currentWave.waveConfig.enemiesPerWave; i++)
            {
                SpawnEnemy(currentWave);
                yield return new WaitForSeconds(currentWave.waveConfig.spawnInterval);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnEnemy(Wave wave)
    {
        GameObject enemyPrefab = wave.waveConfig.enemyPrefabs[Random.Range(0, wave.waveConfig.enemyPrefabs.Length)];
        Vector3 spawnPosition = GetRandomSpawnPosition(wave);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition(Wave wave)
    {
        Vector3 randomDirection = Random.insideUnitSphere * wave.maxSpawnDistance;
        randomDirection += transform.position;
        randomDirection.y = 0;

        float distance = Random.Range(wave.minSpawnDistance, wave.maxSpawnDistance);
        Vector3 finalPosition = transform.position + randomDirection.normalized * distance;

        return finalPosition;
    }
}
