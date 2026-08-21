using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image background, progressBar;

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
        progressBar.fillAmount = (float)enemyDeaths / (float)totalEnemyCount;
    }

    private IEnumerator DrainProgressBarRoutine(float timeBetweenWaves)
    {
        float timer = 4;

        while (timer >= 0)
        {
            float fillAmount = timer / timeBetweenWaves;
            progressBar.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.01f);
            timer -= 0.01f;
        }

        yield return null;
    }

    private void ToggleVisibilty(bool visible)
    { 
        progressBar.gameObject.SetActive(visible);
        background.gameObject.SetActive(visible);
    }
}
