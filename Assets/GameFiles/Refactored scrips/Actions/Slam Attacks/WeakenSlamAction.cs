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

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(WeakenDamageMult, "PlaceHolderWeaken" ),
                new List<BaseCondition> { new TimeCondition(true, hitEntity, WeakenDuration) }), slamColour);
    }
}
// weakenText.GetLocalizedString()