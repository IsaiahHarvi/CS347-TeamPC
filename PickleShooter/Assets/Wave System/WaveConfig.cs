using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shows up in context menu to create wave configs for the enemy spawner

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfig", order = 1)]
public class WaveConfig : ScriptableObject
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs for this wave
    public int enemiesPerWave = 5; // Number of enemies in this wave
    public float spawnInterval = 2f; // Time between each spawn in this wave
}

[System.Serializable]
public class Wave
{
    public WaveConfig waveConfig;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
}
