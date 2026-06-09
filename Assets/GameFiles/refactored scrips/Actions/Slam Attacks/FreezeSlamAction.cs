using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Localization;

[Serializable]
public class FreezeSlamAction : BaseSlamAction
{
    public float FreezeDuration = 1f;
    public float FragileDamageMult = 2f;
    public LocalizedString frozenText;


    public FreezeSlamAction() { }

    public FreezeSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float FreezeDuration) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour)
    {
        this.FreezeDuration = FreezeDuration;
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new FreezeStatus(FragileDamageMult, "PlaceHolderFrozen"),
                new List<BaseCondition> { new TimeCondition(true, FreezeDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new FreezeSlamAction(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, FreezeDuration);
    }
}
// frozenText.GetLocalizedString()