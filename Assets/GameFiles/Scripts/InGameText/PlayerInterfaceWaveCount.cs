using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInterfaceWaveCount : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private int waveCount = 0;
    private float timer = 0;
    private bool newWave = false;


    private void OnEnable()
    {
        EnemyDirector.SpawnWave += NewWave;
    }

    private void OnDisable()
    {
        EnemyDirector.SpawnWave -= NewWave;
    }

    private void Awake()
    {
        Text.alpha = 0f;
    }

    private void NewWave(List<EnemyTypes> Enemies)
    {
        IncrementWaveCount();
        DisplayWaveCount();
        newWave = true;
    }

    public void IncrementWaveCount()
    { 
        this.waveCount++;
    }

    public void DisplayWaveCount()
    {
        timer = 0;
        Text.alpha = 0;
        Text.text = "WAVE " + waveCount;
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
        Text.alpha += 0.5f * Time.deltaTime;        
    }

    private void FadeOut()
    {
        if (!(timer >= 2)) { return; }
        
        Text.alpha -= 0.5f * Time.deltaTime;

        if (Text.alpha <= 0)
        {
            newWave = false;
        }
    }
}
