using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerInterfaceWaveNumber : StaticText
{
    private int waveCount = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        WaveManager.DisplayWaveNumber += NewWave;
        WaveManager.WaveOver += FadeOut;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        WaveManager.DisplayWaveNumber -= NewWave;
        WaveManager.WaveOver -= FadeOut;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 0f;
    }

    private void NewWave(int waveNumber)
    {
        waveCount = waveNumber;
        tmpAsset.alpha = 0;
        UpdateText(null);
        FadeIn(0);
    }

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString() + " " + waveCount;
    }

    private void FadeIn(float timeBetweenWaves)
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (tmpAsset.alpha < 1)
        {
            tmpAsset.alpha = Mathf.Clamp01(tmpAsset.alpha + (1f * Time.deltaTime));
            yield return null;
        }
    }

    private void FadeOut(float timeBetweenWaves)
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        while (tmpAsset.alpha > 0)
        {
            tmpAsset.alpha -= 2f * Time.deltaTime;
            yield return null;
        }
    }
}
