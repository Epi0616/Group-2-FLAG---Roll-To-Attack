using System.Collections.Generic;
using UnityEngine;

public class EnhancedWeakenStatus : WeakenStatus
{
    private bool pulseProcced;
    private LayerMask hostileMask;
    private int enhancementLevel = 1;
    public EnhancedWeakenStatus(float weakMultiplier, string effectText, LayerMask hostileMask, int enhancementLevel) : base(weakMultiplier, effectText)
    {
        type = StatusType.Weak;
        this.effectColour = Color.darkMagenta;
        this.hostileMask = hostileMask;
        this.enhancementLevel = enhancementLevel;
        isStackable = true;
        pulseProcced = false;
    }
    /*
    protected override void ApplyStatModifier()
    {
        enemyRef.damageTakenModifierStat.AddMultiplierFlat(weakMultiplier);
    }
    */

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (!pulseProcced)
        {
            Collider[] hitColliders = new Collider[10];
            int numHit = Physics.OverlapSphereNonAlloc(entityRef.transform.position, 3 + enhancementLevel, hitColliders, hostileMask);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("StaticEntity") || collider.CompareTag("PhysicsEntity")) { return; }
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                if ( hitEntity = entityRef) { return; }

                hitEntity.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(0.2f, effectText),
                new List<BaseCondition> { new TimeCondition(true, 5f) }, true), effectColour);
            }
            pulseProcced = true;
        }
        
        if (type == DamageType.Weaken)
        {
            return;
        }

        entityRef.OnTakeDamage((int)(damage.GetFinalValue() * (weakMultiplier - 1)), effectColour, DamageType.Weaken);

    }
}
