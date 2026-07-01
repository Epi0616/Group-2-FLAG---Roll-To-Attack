using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnhancedKnockbackSlam : BaseSlamAction , IEnhancedAbility
{
    public float CrumblingDamageMod = 1.4f;
    public int enhancementLevel { get; set; }

    public EnhancedKnockbackSlam() { }

    public EnhancedKnockbackSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float CrumblingMod, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        CrumblingDamageMod = CrumblingMod;
        this.enhancementLevel = enhancementLevel;
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 1.25f) },
            true));
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new EnhancedCrumblingStatus(CrumblingDamageMod, slamColour, ownerEntity, enhancementLevel),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 1.25f) },
            true));
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedKnockbackSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, CrumblingDamageMod, preventsMovement, enhancementLevel);
    }
}
