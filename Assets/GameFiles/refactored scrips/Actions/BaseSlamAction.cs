using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseSlamAction : BaseEntityAction
{
    protected ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    private int damage = 5;
    private bool triggered = false;
    private Vector3 slamOrigin;

    public BaseSlamAction() { }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        slamVariablesAccess = entity as ISlamActionRequirements;
        preventsMovement = true;
        triggered = false;
        //Debug.Log("STARTED");
        slamOrigin = ownerEntity.transform.position + (ownerEntity.transform.forward * slamVariablesAccess.slamPositionOffset.z) + (ownerEntity.transform.right * slamVariablesAccess.slamPositionOffset.x);// + ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        EnemyAttackImpactField field = slamVariablesAccess.SPAWNTHING(slamVariablesAccess.DebugSlamObj, slamOrigin).GetComponent<EnemyAttackImpactField>();
        field.PassInValuesColorRadiusLifeTimeChargeTime(Color.red, slamVariablesAccess.slamBaseRange, slamVariablesAccess.slamChargeUpTime + 1f, 1.5f);
    }

    public override void UpdateAction()
    {
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > slamVariablesAccess.slamChargeUpTime && !triggered)
        {
            triggered = true;
           // Debug.Log("SLAMMING");
            Slam();
        }
    }
    public override void FixedUpdateAction()
    {
    }
    public override void InterruptAction()
    {
    }
    public override void EndAction()
    {
    }

    public virtual void Slam()
    {
        
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 20f, slamVariablesAccess.environmentMask))
        {
            
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamVariablesAccess.slamBaseRange, ownerEntity.hostileMask);
            foreach (var collider in colliders)
            {

                if (collider.gameObject == ownerEntity.gameObject) { continue; }
                //Debug.Log("HIT A THING");
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                if (hitEntity == null) { continue; }

                hitEntity.OnTakeDamage(10, Color.white , DamageType.Normal);
            }
        }
    }

    public virtual void ProcessHits(Collider[] colliders, RaycastHit hit) { }
}
// slamVariablesAccess.defaultSlamColour