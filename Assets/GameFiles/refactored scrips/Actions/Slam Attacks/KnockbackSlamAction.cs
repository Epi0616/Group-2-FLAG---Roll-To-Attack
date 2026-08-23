using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KnockbackSlamAction : BaseSlamAction , IUpgradableAbility
{
    public float CrumblingDamageMod = 1.4f;
    
    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public KnockbackSlamAction() { }

    public KnockbackSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float CrumblingMod, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        CrumblingDamageMod = CrumblingMod;
        upgradeResult = result;
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        base.ApplyCustomEffectPerEntity(hitEntity);
        if (!(slamRange.GetFinalValue() > slamRange.GetBaseValue()))
        {
            hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
        }
        
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new CrumblingStatus(CrumblingDamageMod),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
    }

    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        float percentage = slamRange.GetFinalValue() / slamRange.GetBaseValue();
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f * percentage),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true),
            Color.red);
    }

    public override BaseEntityAction Clone()
    {
        return new KnockbackSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, CrumblingDamageMod, preventsMovement, upgradeResult);
    }
}
