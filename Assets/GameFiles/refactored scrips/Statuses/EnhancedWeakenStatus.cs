using System.Collections.Generic;
using UnityEngine;

public class EnhancedWeakenStatus : WeakenStatus, IEnhancedStatusEffect
{
    private bool pulseProcced;
    public int enhancementLevel { get; set; }
    private Entity applierEntity;
    public EnhancedWeakenStatus(float weakMultiplier, string effectText, Entity EntityThatApplied, int enhancementLevel) : base(weakMultiplier, effectText)
    {
        pulseProcced = false;
        type = StatusType.Weak;
        this.effectColour = Color.darkViolet; 
        applierEntity = EntityThatApplied;
        this.enhancementLevel = enhancementLevel;
        isStackable = false;
        
    }
    /*
    protected override void ApplyStatModifier()
    {
        enemyRef.damageTakenModifierStat.AddMultiplierFlat(weakMultiplier);
    }
    */

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {

        if (type == DamageType.Weaken)
        {
            return;
        }

        if (!pulseProcced)
        {
            pulseProcced = true;
            //Debug.Log("Procced");
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
                if ( hitEntity == entityRef) { continue; }
                if ( hitEntity == null ) { continue; }
                //Debug.Log("Weaken Burst");
                hitEntity.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(1.2f, effectText),
                new List<BaseCondition> { new TimeCondition(true, 5f) }, true), Color.darkMagenta);
            }
            
        }

        entityRef.OnTakeDamage((int)(damage.GetFinalValue() * (weakMultiplier - 1)), effectColour, DamageType.Weaken);

    }
}
