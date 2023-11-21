using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    public TextMeshProUGUI waveText;
    public EnemySpawner spawner;

    private GameObject[] enemies;
    private int waveIndex;
    private bool waveEnd, spawnEnd;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        waveEnd = false;
        spawnEnd = false;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (waveIndex < waves.Length)
        {
            Wave currentWave = waves[waveIndex];
            waveEnd = false;
            spawnEnd = false;
            for (int i = 0; i < currentWave.waveConfig.enemiesPerWave; i++)
            {
                spawner.SpawnEnemy(currentWave);
                yield return new WaitForSeconds(currentWave.waveConfig.spawnInterval);
            }
            spawnEnd = true;
            
            while(!waveEnd) //Waits until waveEnd (All the enemies have been killed)
            {
                yield return new WaitForSeconds(1f);
            }
            waveIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Pickle");
        waveText.text = "Wave: " + (waveIndex + 1) + "\nRemaining: " + enemies.Length;
        if(spawnEnd && enemies.Length == 0) waveEnd = true;
    }
}
