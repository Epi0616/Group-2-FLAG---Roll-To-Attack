using UnityEngine;
using UnityEngine.Localization;
using System;
using System.Collections.Generic;

[Serializable]
public class WeakenSlamAction : BaseSlamAction, IUpgradableAbility
{
    public float WeakenDuration = 1f;
    public float WeakenDamageMult = 2f;
    public LocalizedString weakenText;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public WeakenSlamAction() { }

    public WeakenSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float WeakenDuration, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.WeakenDuration = WeakenDuration;
        upgradeResult = result;
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        //hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Weaken);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new WeakenStatus(WeakenDamageMult, "PlaceHolderWeaken" ),
                new List<BaseCondition> { new TimeCondition(true, WeakenDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new WeakenSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, WeakenDuration, preventsMovement, upgradeResult);
    }
}
// weakenText.GetLocalizedString()