using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SmokeParticle : ParticleEffectInstance
{
    //public AnimationCurve clipCurve;

    public override void PlayParticleEffect(EffectSettings settings)
    {
        this.settings = settings;
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;
        StartCoroutine(PlayParticleSequence());
    }

    public override IEnumerator PlayParticleSequence()
    {

        settings.ApplyEffectOverrides(particleSystem);
        particleSystem.Play();

        StartCoroutine(HandleSmokeFade());

        yield return null;
       
        settings.ApplyLateEffectOverrides(particleSystem);

    }

    public override void Update()
    {
        base.Update();
    }

    public IEnumerator HandleSmokeFade()
    {
        
        float fadeTimer = 0;
        float duration = main.startLifetime.constantMax;
        while (fadeTimer < duration)
        {
            renderer.GetPropertyBlock(block);
            fadeTimer += Time.deltaTime;
            block.SetFloat("_AlphaClip", Mathf.Clamp01(Mathf.Lerp(0, 1, (fadeTimer / duration))));
            renderer.SetPropertyBlock(block);
            yield return null;
        }
    }
}
