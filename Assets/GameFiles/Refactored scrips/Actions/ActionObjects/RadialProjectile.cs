using System.Collections.Generic;
using UnityEngine;

public class RadialProjectile : MonoBehaviour, IDecalShadowCast
{
    private Vector3 startPos;
    private Entity ownerEntity;
    private float distance;
    public Stat speed;

    private IRadialProjectile radialProjectile;
    private bool active = false;

    public ShadowDecal currentShadowDecal { get; set; }
    [SerializeField] private GameObject ShadowDecalPrefab;
    public GameObject shadowDecalPrefab { get => ShadowDecalPrefab; set => ShadowDecalPrefab = value; }

    public void Initialize(Entity ownerEntity, float distance, float speed)
    {
        this.speed.ResetModifiers();
        this.ownerEntity = ownerEntity;
        this.distance = distance;
        this.speed = new Stat(speed);

        if (!(ownerEntity is IRadialProjectile radialProjectile)) { Debug.LogError("owner entity is not of type IRadialProjectile"); return; }
        this.radialProjectile = radialProjectile;

        startPos = transform.position;
        active = true;
    }

    private void Update()
    {
        if (!active) return;

        FlyToTarget();
        CheckForDistanceComplete();
    }

    private void FlyToTarget()
    {
        transform.position += transform.forward * speed.GetFinalValue() * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!active) return;

        if ((radialProjectile.radialTargetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            OnHit(hit.gameObject);
        }
    }

    private void OnHit(GameObject hit)
    {
        if (!hit.TryGetComponent<Entity>(out Entity entity)) return;

        entity.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(transform.position, 5f), new List<BaseCondition>() { new AlwaysTrueCondition() }, true));
        entity.OnTakeDamage(4, Color.red, DamageType.Spell);
    }

    private void CheckForDistanceComplete()
    {
        if ((transform.position - startPos).magnitude > distance)
        {
            OnDistanceReached();
        }
    }

    private void OnDistanceReached()
    {
        radialProjectile = null;
        speed.ResetModifiers();
        if (currentShadowDecal  != null)
        {
            currentShadowDecal.DestroyMe();
            currentShadowDecal = null;
        }
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), transform.position, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {
                    new BurstCountEffectOverride(new rangePair(3, 5)),
                    new StartLifetimeEffectOverride(new rangePair(0.3f, 0.5f)),
                    new StartSpeedEffectOverride(new rangePair(5, 7)),
                    new ShapeRadiusEffectOverride(2),
                    new ColourEffectOverride(Color.red),
                }));
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
