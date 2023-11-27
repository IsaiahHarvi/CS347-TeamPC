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
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        float distance = Random.Range(wave.minSpawnDistance, wave.maxSpawnDistance);
        Vector3 basePosition = transform.position + direction * distance;

        RaycastHit hit;
        // Cast a ray downwards from the basePosition
        if (Physics.Raycast(basePosition + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity))
        {
            // If it hits something (like terrain), set the y position to the hit point
            basePosition.y = hit.point.y;
        }
        else
        {
            basePosition.y = 25; // Set a default height
        }

        return basePosition;
    }

}
