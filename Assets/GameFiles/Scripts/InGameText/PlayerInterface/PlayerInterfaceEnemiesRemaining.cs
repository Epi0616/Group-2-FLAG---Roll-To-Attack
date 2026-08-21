using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterfaceEnemiesRemaining : MonoBehaviour
{
    public Image progress;

    private int enemyDeaths = 0;
    private int totalEnemyCount = 1;

    private void OnEnable()
    {
        WaveBuilder.EnemiesGenerated += HandleEnemiesGenerated;
        DicePedestal.WaveStartPedestal += StartDrainProgressBarRoutine;
        EnemyHealthSystem.EnemyHasDied += EnemyHasDied;
    }

    private void OnDisable()
    {
        WaveBuilder.EnemiesGenerated -= HandleEnemiesGenerated;
        DicePedestal.WaveStartPedestal -= StartDrainProgressBarRoutine;
        EnemyHealthSystem.EnemyHasDied -= EnemyHasDied;
    }

    private void HandleEnemiesGenerated(int enemyCount)
    { 
        totalEnemyCount = enemyCount;
        enemyDeaths = 0;
    }

    private void StartDrainProgressBarRoutine(float timeBetweenWaves)
    {
        StartCoroutine(DrainProgressBarRoutine(timeBetweenWaves));
    }

    private void EnemyHasDied()
    {
        enemyDeaths++;
        progress.fillAmount = (float)enemyDeaths / (float)totalEnemyCount;
    }

    private IEnumerator DrainProgressBarRoutine(float timeBetweenWaves)
    {
        float timer = 4;

        while (timer >= 0)
        {
            float fillAmount = timer / timeBetweenWaves;
            progress.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.01f);
            timer -= 0.01f;
        }

        yield return null;
    }
}
