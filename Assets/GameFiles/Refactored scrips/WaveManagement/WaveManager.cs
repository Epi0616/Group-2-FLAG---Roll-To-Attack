using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static event Action<float> WaveOver;
    public static event Action<float> WaveCountStart;
    public static event Action<int> DisplayWaveNumber;

    [SerializeField] private WaveBuilder waveBuilder;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private float delayBetweenWaves = 5f;
    [SerializeField] private int currentWaveIndex = 0;

    private int enemiesLeftInWave = 0;

    private void OnEnable()
    {
        WaveBuilder.EnemiesGenerated += HandleEnemiesGenerated;
        EnemyHealthSystem.EnemyHasDied += HandleEnemyDeath;
        DicePedestal.WaveStartPedestal += StartNextWave;
        TutorialManager.StartIndexWave += StartIndexedWave;
    }

    private void OnDisable()
    {
        WaveBuilder.EnemiesGenerated -= HandleEnemiesGenerated;
        EnemyHealthSystem.EnemyHasDied -= HandleEnemyDeath;
        DicePedestal.WaveStartPedestal -= StartNextWave;
        TutorialManager.StartIndexWave -= StartIndexedWave;
    }

    private void HandleEnemiesGenerated(int enemiesInWave)
    {
        enemiesLeftInWave = enemiesInWave;
        Debug.Log(enemiesLeftInWave + " enemies in this wave");
    }

    private void HandleEnemyDeath()
    { 
        enemiesLeftInWave --;
        RunTimeStatTracker.totalEnemiesKilled += 1;
        if (enemiesLeftInWave <= 0)
        {
            WaveOver?.Invoke(delayBetweenWaves);
        }
    }

    private void StartNextWave(float delayBetweenWaves)
    {
        WaveCountStart?.Invoke(delayBetweenWaves);
        StartCoroutine(SpawnWaveDelay());
    }

    private void StartIndexedWave(int index)
    {
        //Debug.Log("Spawning Indexed Wave");
        Wave randomWave = waveBuilder.GetNextWave(index);
        waveSpawner.SpawnWave(randomWave);       
    }

    private IEnumerator SpawnWaveDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        currentWaveIndex++;
        Wave randomWave = waveBuilder.GetNextWave(currentWaveIndex);
        waveSpawner.SpawnWave(randomWave);
        DisplayWaveNumber?.Invoke(currentWaveIndex);
    }
}
