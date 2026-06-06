using UnityEngine;
using System;

[Serializable]
public class BaseSlamAction : BaseEntityAction
{
    protected ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    private int damage = 5;
    

    public BaseSlamAction() { }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        slamVariablesAccess = entity as ISlamActionRequirements;
        preventsMovement = true;

    }

    public override void UpdateAction()
    {
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > slamVariablesAccess.slamChargeUpTime)
        {
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
        Vector3 slamOrigin = ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 20f, slamVariablesAccess.environmentMask))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamVariablesAccess.slamBaseRange, ownerEntity.hostileMask);
            foreach (var collider in colliders)
            {
                if (collider.gameObject == ownerEntity.gameObject) { continue; }

                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                if (hitEntity == null) { continue; }

                hitEntity.OnTakeDamage(10, Color.white , DamageType.Normal);
            }
        }
    }
}
// slamVariablesAccess.defaultSlamColour