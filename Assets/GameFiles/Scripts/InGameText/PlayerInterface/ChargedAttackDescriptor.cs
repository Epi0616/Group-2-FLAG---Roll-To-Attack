using System.Collections;
using UnityEngine;

public class ChargedAttackDescriptor : StaticText
{

    protected override void OnEnable()
    {
        base.OnEnable();
        //DiceFaceSelectionUIManager.DiceFaceSelectionOver += HandleSelectionPhaseOver;
        DicePedestal.ChargeTextAppear += HandleTextAppear;
        DicePedestal.WaveAutoStartPedestal += HandleWaveStart;
        DicePedestal.WaveHeavyStartPedestal += HandleWaveStart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HandleSelectionPhaseOver;
        DicePedestal.ChargeTextAppear -= HandleTextAppear;
        DicePedestal.WaveAutoStartPedestal += HandleWaveStart;
        DicePedestal.WaveHeavyStartPedestal -= HandleWaveStart;
    }

    protected override void Awake()
    {
        base.Awake();
        tmpAsset.alpha = 1f;
    }

    private void HandleTextAppear(float time)
    {
        StartCoroutine(FadeIn());
    }

    private void HandleWaveStart(float time)
    {
        StartCoroutine(FadeOut());
    }

    protected override void UpdateText(string newText)
    {
        tmpAsset.text = localizedString.GetLocalizedString();
    }

    private IEnumerator FadeIn()
    {
        float timer = 1;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            tmpAsset.alpha = Mathf.Clamp01(1 - timer);
            yield return null;
        }

        tmpAsset.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float timer = 1;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < tmpAsset.alpha)
            {
                tmpAsset.alpha = Mathf.Clamp01(timer);
            }
            yield return null;
        }

        tmpAsset.alpha = 0;
    }
}