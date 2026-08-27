using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static event Action<WaveType> UpdateWaveBar;
    public static event Action<float> WaveOver;
    public static event Action<float> WaveCountStart;
    public static event Action<int> DisplayWaveNumber;

    [Header("Setup")]
    [SerializeField] private WaveBuilder waveBuilder;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private WaveScaling waveScaling;
    [SerializeField] private int currentWaveIndex = 0;

    private int enemiesLeftInWave = 0;
    [SerializeField] private bool spawningWave = false;

    private void OnEnable()
    {
        SpawnWaveAction.SpawnWaveRequest += SpawnWaveWithBudget;
        FireballRainAction.SpawnWaveRequest += SpawnWave;
        WaveBuilder.EnemiesGenerated += HandleEnemiesGenerated;
        EnemyHealthSystem.EnemyHasDied += HandleEnemyDeath;
        DicePedestal.WaveAutoStartPedestal += StartNextWave;
        DicePedestal.WaveHeavyStartPedestal += StartNextWave;
        TutorialManager.StartIndexWave += StartIndexedWave;

        WaveSpawner.finishedSpawning += HandleFinishedSpawning;
    }

    private void OnDisable()
    {
        SpawnWaveAction.SpawnWaveRequest -= SpawnWaveWithBudget;
        FireballRainAction.SpawnWaveRequest -= SpawnWave;
        WaveBuilder.EnemiesGenerated -= HandleEnemiesGenerated;
        EnemyHealthSystem.EnemyHasDied -= HandleEnemyDeath;
        DicePedestal.WaveAutoStartPedestal += StartNextWave;
        DicePedestal.WaveHeavyStartPedestal -= StartNextWave;
        TutorialManager.StartIndexWave -= StartIndexedWave;

        WaveSpawner.finishedSpawning -= HandleFinishedSpawning;
    }

    private void HandleFinishedSpawning()
    { 
        spawningWave = false;
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
            WaveOver?.Invoke(2);
        }
    }

    private void StartNextWave(float delayBetweenWaves)
    {
        if (!spawningWave)
        {
            WaveCountStart?.Invoke(delayBetweenWaves);
            spawningWave = true;
        }
        StartCoroutine(SpawnWaveDelay(delayBetweenWaves));
    }

    private void StartIndexedWave(int index)
    {
        //Debug.Log("Spawning Indexed Wave");
        Wave randomWave = waveBuilder.GetNextWave(index);
        waveSpawner.SpawnWave(randomWave, true);       
    }

    private IEnumerator SpawnWaveDelay(float delayBetweenWaves)
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        spawningWave = true;
        currentWaveIndex++;

        Wave randomWave = waveBuilder.GetNextWave(currentWaveIndex);

        waveScaling.UpdateScaling(currentWaveIndex);
        waveSpawner.SpawnWave(randomWave, true);

        UpdateWaveBar?.Invoke(randomWave.waveType);
        DisplayWaveNumber?.Invoke(currentWaveIndex);

        if (PlayerPrefsManager.instance?.GetInt(PlayerValues.HighScore) < currentWaveIndex)
        {
            PlayerPrefsManager.instance?.SetInt(PlayerValues.HighScore, currentWaveIndex);
        }
    }

    public void SpawnWaveWithBudget(int wave, int budget)
    {
        if (!spawningWave)
        {
            WaveCountStart?.Invoke(0);
            spawningWave = true;
        }

        Wave newWave = waveBuilder.GenerateWave(wave, budget);
        waveSpawner.SpawnWave(newWave, false);
    }

    public void SpawnWave(WaveObj waveObj)
    {
        if (!spawningWave)
        {
            WaveCountStart?.Invoke(0);
            spawningWave = true;
        }

        Wave newWave = waveBuilder.UnpackWaveObj(waveObj);
        waveSpawner.SpawnWave(newWave, false);
    }
}
