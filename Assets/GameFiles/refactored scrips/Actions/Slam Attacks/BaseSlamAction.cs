using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseSlamAction : BaseEntityAction
{
    protected ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    public int slamDamage = 10;
    public Color slamColour = Color.white;
    public float chargeTime = 1f;
    private bool chargeComplete = false;
    protected Vector3 slamOrigin;
    protected bool attackInterrupted = false;
    protected ImpactFieldVisual impactField;
    public float slamRange = 0;
    public Vector3 slamPositionOffset = Vector3.zero;
    //public GameObject slamImpactField;

    public BaseSlamAction() { }
    public BaseSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour)
    {
        this.slamDamage = slamDamage;
        this.chargeTime = chargeTime;
        this.slamRange = slamRange;
        this.slamPositionOffset = slamPositionOffset;
        this.slamColour = slamColour;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        preventsMovement = true;
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
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.SlamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange, chargeTime, true);
    }

    public virtual void SpawnSlamCompleteVFX() { }

    public override void UpdateAction()
    {
        if (attackInterrupted) { return; }
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > chargeTime && !chargeComplete)
        {
            chargeComplete = true;
            SpawnSlamCompleteVFX();
            ExtraSlamEffect();
           // Debug.Log("SLAMMING");
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
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 20f, slamVariablesAccess.environmentMask))
        {
            
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange, ownerEntity.hostileMask);
            ProcessHits(colliders, hit);
        }
    }

    public virtual void ProcessHits(Collider[] colliders, RaycastHit hit)
    {
        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            //Debug.Log("HIT A THING");
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
            
        }
        EndAction();
    }

    public virtual void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
       // hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
    }

    public virtual void ExtraSlamEffect() { }

    public override BaseEntityAction Clone()
    {
        return new BaseSlamAction(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour);
    }
}
// slamVariablesAccess.defaultSlamColour