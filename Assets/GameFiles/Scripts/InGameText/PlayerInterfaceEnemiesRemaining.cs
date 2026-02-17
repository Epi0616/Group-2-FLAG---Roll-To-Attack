using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInterfaceEnemiesRemaining : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private float timer = 0, timeToNextWave = 0;
    private int enemyCount = 0;
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
        enemyCount = totalEnemies.Count;
        timer = 0;
        Text.alpha = 0;
        waveInProgress = true;
    }

    private void EnemyHasDied()
    {
        enemyCount--;
    }

    public void DisplayRemainingEnemies()
    {
        Text.text = "ENEMIES REMAINING " + enemyCount;
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
