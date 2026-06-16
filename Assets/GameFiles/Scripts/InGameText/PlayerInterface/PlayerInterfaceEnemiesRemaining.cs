using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerInterfaceEnemiesRemaining : StaticText
{
    public Image progress;

    private float timer = 0;
    private int enemyDeaths = 0;
    private int totalEnemyCount = 1;
    private bool waveInProgress = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        OldEnemyDirector.SpawnWave += NewWave;
        DicePedestal.WaveStartPedestal += StartDrainProgressBarRoutine;
        EnemyHealthSystem.EnemyHasDied += EnemyHasDied;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OldEnemyDirector.SpawnWave -= NewWave;
        DicePedestal.WaveStartPedestal -= StartDrainProgressBarRoutine;
        EnemyHealthSystem.EnemyHasDied -= EnemyHasDied;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 0f;
    }

    private void NewWave(List<EnemyTypes> totalEnemies)
    {
        totalEnemyCount = totalEnemies.Count;
        enemyDeaths = 0;
        timer = 0;
        tmpAsset.alpha = 0;
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

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString() + " " + enemyDeaths;
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
        if (!(enemyDeaths >= totalEnemyCount)) { return; }

        tmpAsset.alpha -= 2f * Time.deltaTime;

        if (tmpAsset.alpha <= 0)
        {
            waveInProgress = false;
        }
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
