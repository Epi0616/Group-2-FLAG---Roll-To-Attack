using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerInterfaceEnemiesRemaining : StaticText
{
    public Image progress;

    private float timer = 0;
    private int enemyCount = 0;
    private int totalEnemyCount = 1;
    private bool waveInProgress = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyDirector.SpawnWave += NewWave;
        EnemyStateController.EnemyHasDied += EnemyHasDied;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyDirector.SpawnWave -= NewWave;
        EnemyStateController.EnemyHasDied -= EnemyHasDied;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 0f;
    }

    private void NewWave(List<EnemyTypes> totalEnemies)
    {
        totalEnemyCount = totalEnemies.Count;
        enemyCount = totalEnemies.Count;
        timer = 0;
        tmpAsset.alpha = 0;
        waveInProgress = true;
        progress.fillAmount = (float)enemyCount / (float)totalEnemyCount;
    }

    private void EnemyHasDied()
    {
        enemyCount--;
        progress.fillAmount = (float)enemyCount / (float)totalEnemyCount;
    }

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString() + " " + enemyCount;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (waveInProgress)
        {
            FadeIn();
            UpdateText(localizedString.GetLocalizedString());
            FadeOut();
        }
    }

    private void FadeIn()
    {
        if (!((timer <= 2) && (timer >= 1))) { return; }
        tmpAsset.alpha = Mathf.Clamp01(tmpAsset.alpha + (1f * Time.deltaTime));
    }

    private void FadeOut()
    {
        if (!(enemyCount <= 0)) { return; }

        tmpAsset.alpha -= 2f * Time.deltaTime;

        if (tmpAsset.alpha <= 0)
        {
            waveInProgress = false;
        }
    }
}
