using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System;

[Serializable]
public class EnhancedWeakenSlam : BaseSlamAction, IEnhancedAbility
{
    public float WeakenDuration = 4f;
    public float WeakenDamageMult = 2f;
    public LocalizedString weakenText;
    public int enhancementLevel { get; set; }
    public EnhancedWeakenSlam() { }

    public EnhancedWeakenSlam(AudioPackage lightImpactNoise, AudioPackage heavyImpactNoise, int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float WeakenDuration, bool DoesPrevent, int enhancementLevel) : base(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.lightImpactNoise = lightImpactNoise;
        this.heavyImpactNoise = heavyImpactNoise;
        this.WeakenDuration = WeakenDuration;
        this.enhancementLevel = enhancementLevel;
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        base.ApplyCustomEffectPerEntity(hitEntity);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new EnhancedWeakenStatus(WeakenDamageMult, "PlaceHolderWeaken", ownerEntity, enhancementLevel),
                new List<BaseCondition> { new TimeCondition(true, WeakenDuration) }, true));
    }
    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        float percentage = slamRange.GetFinalValue() / slamRange.GetBaseValue();
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 1f * percentage),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true),
            Color.red);
    }
    public override BaseEntityAction Clone()
    {
        return new EnhancedWeakenSlam(lightImpactNoise, heavyImpactNoise, slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, WeakenDuration, preventsMovement, enhancementLevel);
    }
}
