using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseSlamAction : BaseEntityAction
{
    protected ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    public int damage = 10;
    public Color slamColour = Color.white;
    public float chargeTime = 1f;
    private bool chargeComplete = false;
    private Vector3 slamOrigin;
    protected bool attackInterrupted = false;
    protected EnemyAttackImpactField impactField;
    public float slamRange = 0;
    public Vector3 slamPositionOffset = Vector3.zero;

    public BaseSlamAction() { }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        preventsMovement = true;
        slamVariablesAccess = entity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

            //Debug.Log("STARTED");
        slamOrigin = ownerEntity.transform.position + (ownerEntity.transform.forward * slamPositionOffset.z) + (ownerEntity.transform.right * slamPositionOffset.x);
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.DebugSlamObj, slamOrigin, Quaternion.identity).GetComponent<EnemyAttackImpactField>();

        // + ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        //EnemyAttackImpactField field = slamVariablesAccess.SPAWNTHING(slamVariablesAccess.DebugSlamObj, slamOrigin).GetComponent<EnemyAttackImpactField>();
        impactField.PassInValuesColorRadiusLifeTimeChargeTime(slamColour, slamRange, chargeTime + 1f, chargeTime);
    }

    public override void UpdateAction()
    {
        if (attackInterrupted) { return; }
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > chargeTime && !chargeComplete)
        {
            chargeComplete = true;
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
            ApplyCustomEffect(hitEntity);
            
        }
        EndAction();
    }

    public virtual void ApplyCustomEffect(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(damage, slamColour, DamageType.Normal);
    }
}
// slamVariablesAccess.defaultSlamColour