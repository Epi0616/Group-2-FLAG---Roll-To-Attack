using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterfaceEnemiesRemaining : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Image progress;

    private float timer = 0;
    private int enemyCount;
    private int totalEnemyCount;
    private bool waveInProgress = false;


    private void OnEnable()
    {
        EnemyDirector.SpawnWave += NewWave;
        EnemyStateController.EnemyHasDied += EnemyHasDied;
    }

    private void OnDisable()
    {
        EnemyDirector.SpawnWave -= NewWave;
        EnemyStateController.EnemyHasDied -= EnemyHasDied;
    }

    private void Awake()
    {
        Text.alpha = 0f;
    }

    private void NewWave(List<EnemyTypes> totalEnemies)
    {
        totalEnemyCount = totalEnemies.Count;
        enemyCount = totalEnemies.Count;
        timer = 0;
        Text.alpha = 0;
        waveInProgress = true;
        progress.fillAmount = (float)enemyCount / (float)totalEnemyCount;
    }

    private void EnemyHasDied()
    {
        enemyCount--;
        progress.fillAmount = (float)enemyCount / (float)totalEnemyCount;
    }

    public void DisplayRemainingEnemies()
    {
        Text.text = "" + enemyCount;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (waveInProgress)
        {
            FadeIn();
            DisplayRemainingEnemies();
            FadeOut();
        }
    }

    private void FadeIn()
    {
        if (!((timer <= 2) && (timer >= 1))) { return; }
        Text.alpha = Mathf.Clamp01(Text.alpha + (1f * Time.deltaTime));
    }

    private void FadeOut()
    {
        if (!(enemyCount <= 0)) { return; }

        Text.alpha -= 2f * Time.deltaTime;

        if (Text.alpha <= 0)
        {
            waveInProgress = false;
        }
    }
}
