using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlowingSlam : BaseSlamAction , IUpgradableAbility
{
    public float SlowDuration = 1f;
    public float SlowMult = 2f;
    //public LocalizedString slowText;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public SlowingSlam() { }

    public SlowingSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float SlowDuration, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.SlowDuration = SlowDuration;
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(SlowMult, "PlaceHolderSlow"),
                new List<BaseCondition> { new TimeCondition(true, SlowDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new WeakenSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, SlowDuration, preventsMovement);
    }
}
