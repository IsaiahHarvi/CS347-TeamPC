using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void SpawnEnemy(Wave wave)
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
