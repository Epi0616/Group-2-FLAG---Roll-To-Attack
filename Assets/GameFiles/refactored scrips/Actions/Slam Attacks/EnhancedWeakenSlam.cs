using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System;

[Serializable]
public class EnhancedWeakenSlam : BaseSlamAction, IEnhancedAbility
{
    public float WeakenDuration = 1f;
    public float WeakenDamageMult = 2f;
    public LocalizedString weakenText;
    public int enhancementLevel { get; set; }
    public EnhancedWeakenSlam() { }

    public EnhancedWeakenSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float WeakenDuration, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.WeakenDuration = WeakenDuration;
        this.enhancementLevel = enhancementLevel;
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        //hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Weaken);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new EnhancedWeakenStatus(WeakenDamageMult, "PlaceHolderWeaken", ownerEntity, enhancementLevel),
                new List<BaseCondition> { new TimeCondition(true, WeakenDuration) }, true));
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedWeakenSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, WeakenDuration, preventsMovement, enhancementLevel);
    }
}
