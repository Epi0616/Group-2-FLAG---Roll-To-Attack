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
    protected MaterialPropertyBlock block;
    protected EffectSettings settings;

    public void Awake()
    {
        renderer = GetComponent<ParticleSystemRenderer>();
        block = new MaterialPropertyBlock();
        defaultState = EffectStateHolder.FetchCurrentState(particleSystem);
    }

    private void ResetParticleToDefault()
    {
        defaultState.ApplyStateToParticleSystem(particleSystem);
    }

    public virtual void PlayParticleEffect(EffectSettings settings)
    {

        this.settings = settings;
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;

        ResetParticleToDefault();

        StartCoroutine(PlayParticleSequence());
        
        
    }

    public virtual IEnumerator PlayParticleSequence()
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

    private IEnumerator DestroySequence()
    {
        settings.RemoveOverrides(particleSystem);
        yield return null;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void DestroyMe()
    {
        if (isDestroyed) { return; }
        isDestroyed = true;
        StartCoroutine(DestroySequence());
    }
}
