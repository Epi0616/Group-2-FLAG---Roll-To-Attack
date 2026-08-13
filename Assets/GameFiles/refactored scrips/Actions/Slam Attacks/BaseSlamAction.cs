using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseSlamAction : BaseEntityAction, ISlam
{
    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset;
  
    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }

    protected ISlamActionRequirements slamVariablesAccess;
    protected float chargeUpTimer = 0;
    protected bool chargeComplete = false;
    protected Vector3 slamOrigin;
    protected bool attackInterrupted = false;
    protected ImpactFieldVisual impactField;
    //public GameObject slamImpactField;

    public BaseSlamAction() { }
    public BaseSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent)
    {
        this.slamDamage = slamDamage;
        this.chargeTime = chargeTime;
        this.slamRange = new Stat(slamRange);
        this.slamPositionOffset = slamPositionOffset;
        this.slamColour = slamColour;
        preventsMovement = DoesPrevent;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        SetupSlam();
        AnimateAttack();
    }
    protected virtual void AnimateAttack()
    {
        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Attack, 1, MixerType.main, 0.2f, chargeTime);
        }
    }
    protected virtual void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

        //slamImpactField = slamVariablesAccess.SlamImpactField;
        // Debug.Log("SLAM STRTED");

        slamOrigin = ownerEntity.transform.position + (ownerEntity.transform.forward * slamPositionOffset.z) + (ownerEntity.transform.right * slamPositionOffset.x);

        // + ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        //EnemyAttackImpactField field = slamVariablesAccess.SPAWNTHING(slamVariablesAccess.DebugSlamObj, slamOrigin).GetComponent<EnemyAttackImpactField>();
        SpawnSlamStartVFX();
    }

    public virtual void SpawnSlamStartVFX()
    {
        if (attackInterrupted) { return; }
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();

        bool flashRed = false;
        if (chargeTime > 0)
        {
            flashRed = true;
        }
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange.GetFinalValue(), chargeTime, flashRed);
    }

    public virtual void SpawnSlamCompleteVFX()
    {
        EffectSettings options = new EffectSettings(
                overrideColour: slamColour,
                overrideScale: new rangePair(1.5f, 1.6f),
                overrideGravity: new rangePair(-0.5f, -1.5f),
                overrideLifetime: new rangePair(0.2f, 0.3f),
                overrideSpeed: new rangePair(25f, 40f));
        if (slamRange.GetFinalValue() > slamRange.GetBaseValue())
        {
            float percentage = slamRange.GetFinalValue() / slamRange.GetBaseValue();
            options.overrideSpeed = new rangePair(25f * percentage, 40f * percentage);
        }
        // Coloured Slam Particles
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SimpleBurst01), slamOrigin, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(options);
        // Smoke Under Dice
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SmokeBurst01), slamOrigin, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideBurstCount: new rangePair(15, 20)));
        // Smoke At Slam Edge
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SmokeBurst01), slamOrigin, Quaternion.Euler(90, 0, 0)).
        //        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideShapeRadius: slamRange.GetFinalValue()));

    }

    public override void UpdateAction()
    {       
        if (attackInterrupted) { impactField.DestroyMe();  return; }
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > chargeTime && !chargeComplete)
        {
            chargeComplete = true;
            SpawnSlamCompleteVFX();
            ExtraSlamEffect();
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyExtraHeavyEffect();
            }
            // Debug.Log("SLAMMING");

            if (slamRange.GetFinalValue() > slamRange.GetBaseValue())
            {
                triggerPillars();
            }
            Slam();
            
        }
    }
    public override void FixedUpdateAction()
    {

    }
    public override void InterruptAction()
    {
        attackInterrupted = true;
        impactField.DestroyMe();
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }

    public virtual void Slam()
    {
        //Debug.Log("Started Slam");
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200f, slamVariablesAccess.groundLayer))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange.GetFinalValue(), ownerEntity.hostileMask);
            ProcessHits(colliders, hit);
        }
    }

    public virtual void triggerPillars()
    {
        //Debug.Log("enter trigger pillars");
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200f, slamVariablesAccess.groundLayer))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange.GetFinalValue(), slamVariablesAccess.pedestalLayer);
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<DicePedestal>().ActivatePedestalWithHeavy();
                //Debug.Log("looping for pillar");
            }
        }
    }

    public virtual void ProcessHits(Collider[] colliders, RaycastHit hit)
    {

        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyHeavyEffectPerEntity(hitEntity);
            }

            //Debug.Log("Processing Loop End");
        }
       // Debug.Log("Slam Ending");
        if (ownerEntity is ISlamActionWithCooldown temp)
        {
            ownerEntity.StartCoroutine(slamCD(temp.cooldownTime));
        }
        else
        {
            EndAction();
        }
            
    }

    public virtual IEnumerator slamCD(float amount)
    {
        yield return new WaitForSeconds(amount);
        EndAction();
    }

    protected virtual void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, 
            true), 
            Color.red);
    }

    protected virtual void ApplyExtraHeavyEffect() { }

    public virtual void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (slamDamage == 0) { return; }
       hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
    }

    public virtual void ExtraSlamEffect() { }

    public override BaseEntityAction Clone()
    {
        return new BaseSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
// slamVariablesAccess.defaultSlamColour