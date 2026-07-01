using System.Collections.Generic;
using UnityEngine;

public class EnhancedCrumblingStatus : CrumblingStatus , IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
    private Entity applierEntity;

    public EnhancedCrumblingStatus(float crumbleMult, Color effectColour, Entity applierEntity, int enhancementLevel) : base(crumbleMult)
    {
        this.enhancementLevel = enhancementLevel;
        this.applierEntity = applierEntity;
        this.effectColour = effectColour;
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Slammed)
        {
            Collider[] hitColliders = new Collider[100];
            int numHit = Physics.OverlapSphereNonAlloc(entityRef.transform.position, 10 + (enhancementLevel * 2), hitColliders, applierEntity.hostileMask);
            if (applierEntity is ISlamActionRequirements temp)
            {
                ImpactFieldVisual field = (ObjectPoolManager.SpawnObject(temp.slamImpactField, entityRef.transform.position, Quaternion.identity)).GetComponent<ImpactFieldVisual>();
                field.PassInValuesColorRadiusChargeTimeFlash(effectColour, 10 + (enhancementLevel * 2), 0, false);
            }
            for (int i = 0; i < numHit; i++)
            {
                Collider collider = hitColliders[i];
                if (collider == null) { return; }
                if (collider.CompareTag("StaticEntity") || collider.CompareTag("PhysicsEntity")) { continue; }
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                if (hitEntity == entityRef) { continue; }
                if (hitEntity == null) { continue; }
                //Debug.Log("Weaken Burst");
                hitEntity.OnTakeDamage(10, Color.sienna, DamageType.Explosive);
            }

            toBeRemoved = true;
            return;
        }
    }
}
