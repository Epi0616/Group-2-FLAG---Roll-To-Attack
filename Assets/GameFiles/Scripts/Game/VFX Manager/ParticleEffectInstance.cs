using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class ParticleEffectInstance : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particleSystem;
    protected float timer;
    protected ParticleSystem.MainModule main;
    protected ParticleSystemRenderer renderer;
    protected EffectStateHolder defaultState;
    protected bool isDestroyed;

    public void Awake()
    {
        renderer = GetComponent<ParticleSystemRenderer>();
        defaultState = EffectStateHolder.FetchCurrentState(particleSystem);
    }

    private void ResetParticleToDefault()
    {
        defaultState.ApplyStateToParticleSystem(particleSystem);
    }

    public virtual void PlayParticleEffect(EffectSettings settings)
    {
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;

        ResetParticleToDefault();

        StartCoroutine(PlayParticleSequence(settings));
        
        
    }

    public virtual IEnumerator PlayParticleSequence(EffectSettings settings)
    {

        settings.ApplyEffectOverrides(particleSystem);
        particleSystem.Play();

        yield return null;

        settings.ApplyLateEffectOverrides(particleSystem);
        
    }

    public virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > main.duration)
        {
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        if (isDestroyed) { return; }
        isDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
