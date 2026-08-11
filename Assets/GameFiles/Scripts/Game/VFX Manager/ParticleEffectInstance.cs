using UnityEngine;

public class ParticleEffectInstance : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private float timer;
    private ParticleSystem.MainModule main;
    private ParticleSystemRenderer renderer;
    private bool isDestroyed;

    public void Awake()
    {
        renderer = GetComponent<ParticleSystemRenderer>();
    }
    public void PlayParticleEffect(EffectSettings settings)
    {
        timer = 0;
        isDestroyed = false;
        main = particleSystem.main;
        settings.ApplySettings(particleSystem, renderer);

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
