using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class ParticleEffectInstance : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particleSystem;
    protected float timer;
    protected ParticleSystem.MainModule main;
    protected ParticleSystemRenderer renderer;
    protected bool isDestroyed;

    public void Awake()
    {
        renderer = GetComponent<ParticleSystemRenderer>();
    }
    public virtual void PlayParticleEffect(EffectSettings settings)
    {
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;
        StartCoroutine(PlayParticleSequence(settings));
        
        
    }

    public virtual IEnumerator PlayParticleSequence(EffectSettings settings)
    {

        settings.ApplySettings(particleSystem, renderer);
        particleSystem.Play();

        yield return null;

        settings.ApplyLateSettings(particleSystem, renderer);
        
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
