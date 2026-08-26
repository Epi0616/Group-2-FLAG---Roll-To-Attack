using UnityEngine;

public class PlayerInterfaceWaveBreakTime : StaticText
{
    private float timer = 0, timeToNextWave = 0;
    private bool waveOver = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        DicePedestal.WaveHeavyStartPedestal += WaitForNextWave;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DicePedestal.WaveHeavyStartPedestal -= WaitForNextWave;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 0f;
    }

    private void WaitForNextWave(float timeToNextWave)
    {
        this.timeToNextWave = timeToNextWave;
        timer = 0;
        tmpAsset.alpha = 0;
        waveOver = true;
    }

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString() + " " + (timeToNextWave - Mathf.FloorToInt(timer));
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (waveOver)
        {
            FadeIn();
            UpdateText(localizedString.GetLocalizedString());
            FadeOut();
        }
    }

    private void FadeIn()
    {
        if (!(timer <= 1)) { return; }
        tmpAsset.alpha = Mathf.Clamp01(tmpAsset.alpha + (1f * Time.deltaTime));
    }

    private void FadeOut()
    {
        if (!(timer >= timeToNextWave)) { return; }
        tmpAsset.alpha -= 2f * Time.deltaTime;

        if (tmpAsset.alpha <= 0)
        {
            waveOver = false;
        }
    }
}