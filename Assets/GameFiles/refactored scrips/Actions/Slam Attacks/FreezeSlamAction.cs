using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Localization;

[Serializable]
public class FreezeSlamAction : BaseSlamAction , IUpgradableAbility
{
    public float FreezeDuration = 1f;
    public float FragileDamageMult = 2f;
    public LocalizedString frozenText;
    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public FreezeSlamAction() { }

    public FreezeSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float FreezeDuration, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.FreezeDuration = FreezeDuration;
        upgradeResult = result;
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new FreezeStatus(FragileDamageMult, "Frozen"),
                new List<BaseCondition> { new TimeCondition(true, FreezeDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new FreezeSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, FreezeDuration, preventsMovement, upgradeResult);
    }
}
// frozenText.GetLocalizedString()