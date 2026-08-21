using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class ArcingProjectile : MonoBehaviour, IDecalShadowCast
{
    [SerializeField] private Rigidbody rb;

    private int slamDamage;
    private Color slamColor;
    private float chargeTime;
    private float slamRange;
    private bool active = false;

    protected Entity ownerEntity;
    private ISlamActionRequirements slamVariablesAccess;
    private Vector3 slamOrigin;
    private bool attackInterrupted = false;
    private ImpactFieldVisual impactField;

    protected bool hasPeaked;

    public ShadowDecal currentShadowDecal { get; set; }
    [SerializeField] private GameObject ShadowDecalPrefab;
    public GameObject shadowDecalPrefab { get => ShadowDecalPrefab; set => ShadowDecalPrefab = value; }


    private void OnEnable()
    {
        active = false;
        rb.isKinematic = true;
        StartCoroutine(LifeTime(7));
    }

    public void HandlePathToTarget(Entity ownerEntity, Vector3 target, float durationOfTravel, int slamDamage, Color slamColor, float slamRange)
    {
        this.ownerEntity = ownerEntity;
        this.slamDamage = slamDamage;
        this.chargeTime = durationOfTravel;
        this.slamColor = slamColor;
        this.slamRange = slamRange;

        Vector3 randomOffset = new(UnityEngine.Random.Range(-10f, 10f), 0f, UnityEngine.Random.Range(-10f, 10f));
        slamOrigin = target + randomOffset;

        SetupSlam();
        StartCoroutine(PathToTarget(slamOrigin, transform.position, durationOfTravel));
        active = true;
    }

    protected virtual IEnumerator LifeTime(float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FallIntoFloor());
        //ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    //Slam
    protected virtual void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        attackInterrupted = false;

        //slamImpactField = slamVariablesAccess.SlamImpactField;
        // Debug.Log("SLAM STRTED");
        

        StartCoroutine(PerformSlam(chargeTime));
    }

    private IEnumerator PerformSlam(float chargeTime)
    {
        //impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        //impactField.PassInValuesColorRadiusChargeTimeFlash(slamColor, slamRange, chargeTime, true);

        while (chargeTime > 0)
        { 
            chargeTime -= Time.deltaTime;
            yield return null;
        }
        Slam();
    }

    public virtual void Slam()
    {
        //Debug.Log("Started Slam");
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200f, slamVariablesAccess.groundLayer))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange, ownerEntity.hostileMask);
            ProcessHits(colliders, hit);
            OnHitEffect();
        }

        Vector3 pos = slamOrigin;
        pos.y += 1.5f;
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {
                    new ColourHueEffectOverride(new rangePair(0.6f, 1f)),
                    new BurstCountEffectOverride(new rangePair(3, 6)),
                    new StartSpeedEffectOverride(new rangePair(5, 10)),
                    new ShapeRadiusEffectOverride(slamRange)
                }));
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst02), pos, Quaternion.Euler(0, 0, 0)).
        //        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { }));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SmokeBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ShapeRadiusEffectOverride(slamRange), new BurstCountEffectOverride(new rangePair(1, 1)) }));
    }

    public virtual void ProcessHits(Collider[] colliders, RaycastHit hit)
    {
        //Debug.Log("Started Processing");
        if (ownerEntity is IJumpable wewa)
        {
            //Debug.Log("Impact Speed is: " + wewa.impactSpeed.GetFinalValue());
        }
        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
        }

    }
    public virtual void OnHitEffect() { }
    public virtual void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (slamDamage == 0) { return; }
        hitEntity.OnTakeDamage(slamDamage, slamColor, DamageType.Spell);
    }

    //Pathing
    protected virtual IEnumerator PathToTarget(Vector3 target, Vector3 initialPosition, float durationOfTravel)
    {
        hasPeaked = false;
        float timer = durationOfTravel;
        float t = 0;

        Vector3 position; 

        float peakInArc = GetPeakInArc(target);
        if(currentShadowDecal != null)
        {
            currentShadowDecal.StartGrowAndShrink(0.5f, durationOfTravel);
        }
        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (durationOfTravel - timer) / durationOfTravel;

            position = Vector3.Lerp(initialPosition, target, t);
            position.y += ArcY(target, initialPosition, peakInArc, t);           
            transform.position = position;

            yield return null;
        }
        transform.position = target;
    }

    protected float ArcY(Vector3 target, Vector3 initialPosition, float peakInArc, float t)
    {
        return peakInArc * 6 * t * (1 - t);
    }

    protected float GetPeakInArc(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        float peakInArc;

        Vector3 midPoint = transform.position + (direction / 2);

        if (distance != 0)
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }
        else
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }

        return peakInArc;
    }

    public void Interrupt()
    {
        if (active) return;

        StopAllCoroutines();
        currentShadowDecal.DestroyMe();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public IEnumerator FallIntoFloor()
    {
        currentShadowDecal.DestroyMe();
        float timer = 0;
        while (timer < 1.5f)
        {
           
            timer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
            yield return null;
        }
       
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    
}
