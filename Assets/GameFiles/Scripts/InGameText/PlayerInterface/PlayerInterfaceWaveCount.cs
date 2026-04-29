using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInterfaceWaveCount : StaticText
{
    private int waveCount = 0;
    private float timer = 0;
    private bool newWave = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyDirector.SpawnWave += NewWave;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyDirector.SpawnWave -= NewWave;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 0f;
    }

    private void NewWave(List<EnemyTypes> Enemies)
    {
        IncrementWaveCount();
        UpdateText(localizedString.GetLocalizedString());
        timer = 0;
        tmpAsset.alpha = 0;
        newWave = true;
    }

    public void IncrementWaveCount()
    { 
        this.waveCount++;
        RunTimeStatTracker.waveNumber = waveCount;
    }

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString() + " " + waveCount;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (newWave)
        {
            FadeIn();
            FadeOut();
        }
 
    }

    private void FadeIn()
    {
        if (!(timer <= 2)) { return; }
        tmpAsset.alpha += 0.5f * Time.deltaTime;        
    }

    private void FadeOut()
    {
        if (!(timer >= 2)) { return; }
        
        tmpAsset.alpha -= 0.5f * Time.deltaTime;

        if (tmpAsset.alpha <= 0)
        {
            newWave = false;
        }
    }
}
