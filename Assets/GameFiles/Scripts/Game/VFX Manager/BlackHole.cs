using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackHole : MonoBehaviour
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private float[] initialRadius = { 10, 7.5f, 5};
    [SerializeField] private GameObject BlackHoleObj;
    private Vector3 initialScale = new Vector3(2.5f, 2.5f, 2.5f);
    private bool isDestroyed;
    private bool enhanced;
    private GameObject parentObj;

    public void AdjustRangePercentage(float percentage)
    {
        for (int i = 0; i < particleSystems.Count - 1; i++)
        {
            ParticleSystem.ShapeModule shape = particleSystems[i].shape;
            shape.radius = initialRadius[i] * (percentage);
        }
    }

    public void Initialize(float range, float lifetime, GameObject parent, bool e)
    {
        enhanced = e;
        parentObj = parent;
        RestoreValues();
        isDestroyed = false;
        AdjustRange(range);
        BlackHoleObj.transform.localScale = initialScale * (range / 10);
    }

    private void Update()
    {
        if (parentObj != null)
        {
            transform.position = parentObj.transform.position;
        }
    }

    public void AdjustRange(float range)
    {
        ParticleSystem.ShapeModule shape = particleSystems[0].shape;
        shape.radius = range;
        shape = particleSystems[1].shape;
        shape.radius = (range * 0.6f);
        shape = particleSystems[2].shape;
        shape.radius = (range * 0.3f);
    }

    public void RestoreValues()
    {
        BlackHoleObj.transform.localScale = initialScale;
        for (int i = 0; i < particleSystems.Count - 1; i++)
        {
            ParticleSystem.ShapeModule shape = particleSystems[i].shape;
            shape.radius = initialRadius[i];
            ParticleSystem.VelocityOverLifetimeModule vel = particleSystems[i].velocityOverLifetime;
            vel.orbitalZ = 1.2f;
            ParticleSystem.EmissionModule em = particleSystems[i].emission;
            em.rateOverTime = 15;
            //ParticleSystem.MainModule main = particleSystems[i].main;
            //main.startSpeed = 1f;
        }
    }
    public void FallIntoCenter()
    {
        for (int i = 0; i < particleSystems.Count - 1; i++)
        {
            ParticleSystem.VelocityOverLifetimeModule vel = particleSystems[i].velocityOverLifetime;
            ParticleSystem.MainModule main = particleSystems[i].main;
            StartCoroutine(FallRoutine(vel.orbitalZ.constant, 0f, 1f, vel, main));
        }
    }

    public void StopEmission()
    {
        for (int i = 0; i < particleSystems.Count - 1; i++)
        {
            ParticleSystem.EmissionModule em = particleSystems[i].emission;
            em.rateOverTime = 0;
        }    
    }

    public void BeginToFade(float delay)
    {
        //StartCoroutine(ShrinkHole(delay));
        StartCoroutine(Collapse(delay * 0.75f, delay * 0.25f));
    }

    private IEnumerator ShrinkHole(float duration)
    {
        float timer = 0;
        Vector3 start = BlackHoleObj.transform.localScale;
        Vector3 end = BlackHoleObj.transform.localScale * 0.1f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            Vector3 z = Vector3.Lerp(start, end, t);
            //float s = Mathf.Lerp(1, -5, z);
            BlackHoleObj.transform.localScale = z;
            //main.startSpeed = s;
            yield return null;
        }
        BlackHoleObj.transform.localScale = end;
    }

    private IEnumerator Collapse(float ExpandDuration, float CollapseDuration)
    {
        float timer = 0;
        Vector3 start = BlackHoleObj.transform.localScale;
        Vector3 expand = start * 1.5f;
        Vector3 end = BlackHoleObj.transform.localScale * 0.1f;
        while (timer < ExpandDuration)
        {
            timer += Time.deltaTime;
            float t = timer / ExpandDuration;
            Vector3 z = Vector3.Lerp(start, expand, t);
            BlackHoleObj.transform.localScale = z;

            yield return null;
        }
        timer = 0;
        while (timer < CollapseDuration)
        {
            timer += Time.deltaTime;
            float t = timer / CollapseDuration;
            Vector3 z = Vector3.Lerp(expand, end, t);
            BlackHoleObj.transform.localScale = z;

            yield return null;
        }
        BlackHoleObj.transform.localScale = end;
    }

    private IEnumerator FallRoutine(float start, float end, float duration, ParticleSystem.VelocityOverLifetimeModule vel, ParticleSystem.MainModule main)
    {
        float timer = 0;
        while (timer  < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float z = Mathf.Lerp(start, end, t);
            //float s = Mathf.Lerp(1, -5, z);
            vel.orbitalZ = z;
            //main.startSpeed = s;
            yield return null;
        }
    }

    public void DestroyMe(float delay)
    {
        if (isDestroyed) return;
        isDestroyed = true;
        StopAllCoroutines();
        parentObj = null;
        StartCoroutine(DestroyRoutine(delay));
    }

    private IEnumerator DestroyRoutine(float delay)
    {
        BeginToFade(delay);
        yield return new WaitForSeconds(delay);
        if (enhanced)
        {
            ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.BlackHole02), transform.position, Quaternion.Euler(90, 0, 0)).
                    GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { }));
        }
        else
        {
            ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.BlackHole01), transform.position, Quaternion.Euler(90, 0, 0)).
                    GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { }));
        }
            
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
