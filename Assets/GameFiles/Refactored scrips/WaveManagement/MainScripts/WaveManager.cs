using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static event Action<WaveType> UpdateWaveBar;
    public static event Action<float> WaveOver;
    public static event Action<float> WaveCountStart;
    public static event Action<int> DisplayWaveNumber;

    public bool waveCleared { get; set; }
    public int currentWaveIndex = 1;

    [Header("Setup")]
    [SerializeField] private WaveBuilder waveBuilder;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private WaveScaling waveScaling;

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

        SetUpArenaManager.SetUpWavePosition += SetUpWaveStateFromIntro;
    }

    private void OnDisable()
    {
        SpawnWaveAction.SpawnWaveRequest -= SpawnWaveWithBudget;
        FireballRainAction.SpawnWaveRequest -= SpawnWave;
        WaveBuilder.EnemiesGenerated -= HandleEnemiesGenerated;
        EnemyHealthSystem.EnemyHasDied -= HandleEnemyDeath;
        DicePedestal.WaveAutoStartPedestal -= StartNextWave;
        DicePedestal.WaveHeavyStartPedestal -= StartNextWave;
        TutorialManager.StartIndexWave -= StartIndexedWave;

        WaveSpawner.finishedSpawning -= HandleFinishedSpawning;

        SetUpArenaManager.SetUpWavePosition -= SetUpWaveStateFromIntro;
    }

    private void SetUpWaveStateFromIntro(int waveIndex, bool cleared)
    {
        if (cleared)
        {
            waveCleared = cleared;
            currentWaveIndex = waveIndex;
            waveScaling?.UpdateScaling(currentWaveIndex);
            WaveOver?.Invoke(0);

            return;
        }

        currentWaveIndex = waveIndex;
        waveScaling?.UpdateScaling(currentWaveIndex);
    }

    private void HandleFinishedSpawning()
    { 
        spawningWave = false;
    }

    private void HandleEnemiesGenerated(int enemiesInWave)
    {
        enemiesLeftInWave = enemiesInWave;
    }

    private void HandleEnemyDeath()
    { 
        enemiesLeftInWave --;
        RunTimeStatTracker.instance.runTimeStats.totalEnemiesKilled += 1;
        if (enemiesLeftInWave <= 0 && !waveCleared)
        {
            waveCleared = true;
            currentWaveIndex++;
            WaveOver?.Invoke(1);
        }
    }

    private void StartNextWave(float delayBetweenWaves)
    {
        if (!spawningWave)
        {
            WaveCountStart?.Invoke(delayBetweenWaves);
            waveCleared = false;
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
        waveCleared = false;

        Wave wave = waveBuilder.GetNextWave(currentWaveIndex);

        waveScaling?.UpdateScaling(currentWaveIndex);
        waveSpawner?.SpawnWave(wave, true);

        UpdateWaveBar?.Invoke(wave.waveType);
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
