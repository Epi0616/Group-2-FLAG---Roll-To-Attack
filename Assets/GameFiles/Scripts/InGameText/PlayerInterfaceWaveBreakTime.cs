using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInterfaceWaveBreakTime : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private float timer = 0, timeToNextWave = 0;
    private bool waveOver = false;


    private void OnEnable()
    {
        EnemyDirector.WaveOver += WaveOver;
    }

    private void OnDisable()
    {
        EnemyDirector.WaveOver -= WaveOver;
    }

    private void Awake()
    {
        Text.alpha = 0f;
    }

    private void WaveOver(float timeToNextWave)
    {
        this.timeToNextWave = timeToNextWave;
        timer = 0;
        Text.alpha = 0;
        waveOver = true;
    }

    public void DisplayTimer()
    {
        Text.text = "NEXT WAVE IN " + (timeToNextWave - Mathf.FloorToInt(timer));
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (waveOver)
        {
            FadeIn();
            DisplayTimer();
            FadeOut();
        }

    }

    private void FadeIn()
    {
        if (!(timer <= 1)) { return; }
        Text.alpha = Mathf.Clamp01(Text.alpha + (1f * Time.deltaTime));
    }

    private void FadeOut()
    {
        if (!(timer >= timeToNextWave)) { return; }

        Text.alpha -= 2f * Time.deltaTime;

        if (Text.alpha <= 0)
        {
            waveOver = false;
        }
    }
}