using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TutorialBasicSlam : BaseSlamAction
{

    public TutorialBasicSlam() { }
    public TutorialBasicSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {

    }

    public override void UpdateAction()
    {
        if (attackInterrupted) { impactField.DestroyMe(); return; }
        chargeUpTimer += Time.deltaTime;
        if (chargeUpTimer > chargeTime && !chargeComplete)
        {
            chargeComplete = true;
            SpawnSlamCompleteVFX();
            
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyExtraHeavyEffect();
            }
            else
            {
                ExtraSlamEffect();
            }
            // Debug.Log("SLAMMING");

            if (slamRange.GetFinalValue() > slamRange.GetBaseValue())
            {
                triggerPillars();
            }
            Slam();
        }
    }

    public override void ProcessHits(Collider[] colliders, RaycastHit hit)
    {

        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyHeavyEffectPerEntity(hitEntity);
            }
            else
            {
                ApplyCustomEffectPerEntity(hitEntity);
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

    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true),
            Color.red);
        if (slamDamage == 0) { return; }
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Heavy);
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (slamDamage == 0) { return; }
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
    }

    public override BaseEntityAction Clone()
    {
        return new TutorialBasicSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
