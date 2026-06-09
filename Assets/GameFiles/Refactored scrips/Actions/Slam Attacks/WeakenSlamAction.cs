using UnityEngine;
using UnityEngine.Localization;
using System;
using System.Collections.Generic;

[Serializable]
public class WeakenSlamAction : BaseSlamAction
{
    public float WeakenDuration = 1f;
    public float WeakenDamageMult = 2f;
    public LocalizedString weakenText;

    public WeakenSlamAction() { }

    public WeakenSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float WeakenDuration, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.WeakenDuration = WeakenDuration;
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(WeakenDamageMult, "PlaceHolderWeaken" ),
                new List<BaseCondition> { new TimeCondition(true, WeakenDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new WeakenSlamAction(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, WeakenDuration, DoesActionPreventMovement);
    }
}
// weakenText.GetLocalizedString()