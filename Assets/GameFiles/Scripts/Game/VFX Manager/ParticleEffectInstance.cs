using UnityEngine;

public class ParticleEffectInstance : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private float timer;
    private ParticleSystem.MainModule main;
    private bool isDestroyed;
    public void PlayParticleEffect(EffectSettings settings)
    {
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;
        settings.ApplySettings(particleSystem);
    
        particleSystem.Play();
    }

    public void Update()
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
