using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class BaseSlamAction : BaseEntityAction, ISlam
{
    [SerializeField] protected int SlamDamage;
    [SerializeField] protected Color SlamColor;
    [SerializeField] protected float ChargeTime;
    [SerializeField] protected Stat SlamRange = new Stat(5);
    [SerializeField] protected Vector3 SlamPositionOffset;
    [SerializeField] protected bool DoesActionPreventMovement;

    public int slamDamage { get => SlamDamage; set => SlamDamage = value; }
    public Color slamColour { get => SlamColor; set => SlamColor = value; }
    public float chargeTime { get => ChargeTime; set => ChargeTime = value; }
    public Stat slamRange { get => SlamRange; set => SlamRange = value; }
    public Vector3 slamPositionOffset { get => SlamPositionOffset; set => SlamPositionOffset = value; }
    public bool doesActionPreventMovement { get => DoesActionPreventMovement; set => DoesActionPreventMovement = value; }

    protected ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    private bool chargeComplete = false;
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
        
        slamVariablesAccess = entity as ISlamActionRequirements;
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

    public virtual void SpawnSlamCompleteVFX() { }

    public override void UpdateAction()
    {       
        if (attackInterrupted) { impactField.DestroyMe();  return; }
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > chargeTime && !chargeComplete)
        {
            chargeComplete = true;
            SpawnSlamCompleteVFX();
            ExtraSlamEffect();
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
            if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyHeavyEffect(hitEntity);
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

    protected virtual void ApplyHeavyEffect(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, 
            true), 
            Color.red);
    }

    public virtual void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
       hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
    }

    public virtual void ExtraSlamEffect() { }

    public override BaseEntityAction Clone()
    {
        return new BaseSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, DoesActionPreventMovement);
    }
}
// slamVariablesAccess.defaultSlamColour