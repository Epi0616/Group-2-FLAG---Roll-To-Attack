using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoraleBoostSlamAction : BaseSlamAction
{
    public float buffDuration = 1f;
    public float speedBoost = 1.5f;
    public int healAmount = 15;

    public MoraleBoostSlamAction() { }
    public MoraleBoostSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour,
        float buffDuration, float speedBoost, int healAmount, bool DoesPrevent)
        : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.buffDuration = buffDuration;
        this.speedBoost = speedBoost;
        this.healAmount = healAmount;
    }

    public override void Slam()
    {
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200f, slamVariablesAccess.groundLayer))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange.GetFinalValue(), 1 << ownerEntity.gameObject.layer);
            ProcessHits(colliders, hit);
        }
    }

    public override void ProcessHits(Collider[] colliders, RaycastHit hit)
    {
        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;      
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);         
        }
   
        if (ownerEntity is ISlamActionWithCooldown temp)
        {
            ownerEntity.StartCoroutine(slamCD(temp.cooldownTime));
        }
        else
        {
            EndAction();
        }
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnRecieveHeal(healAmount, Color.green);
        if (hitEntity == ownerEntity) { return; }
        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new MovementSpeedStatus(speedBoost),
                new List<BaseCondition> { new TimeCondition(true, buffDuration) }, true));
    }

    public override BaseEntityAction Clone()
    {
        return new MoraleBoostSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, buffDuration, speedBoost, healAmount, preventsMovement);
    }

}

