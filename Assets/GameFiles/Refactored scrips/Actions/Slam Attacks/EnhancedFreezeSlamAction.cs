using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Localization;

[Serializable]
public class EnhancedFreezeSlamAction : BaseSlamAction , IEnhancedAbility
{
    public float FreezeDuration = 5f;
    public float FragileDamageMult = 2f;
    public LocalizedString frozenText;
    //[SerializeField] private int EnhancementLevel;
    public int enhancementLevel { get; set; }
    public EnhancedFreezeSlamAction() { }

    public EnhancedFreezeSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float FreezeDuration, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.FreezeDuration = FreezeDuration;
        this.enhancementLevel = enhancementLevel;
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new EnhancedFreezeStatus(FragileDamageMult, "Frozen", slamColour, enhancementLevel),
                new List<BaseCondition> { new TimeCondition(true, FreezeDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedFreezeSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, FreezeDuration, preventsMovement, enhancementLevel);
    }
}
