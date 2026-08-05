using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlowingSlam1 : BaseSlamAction , IUpgradableAbility
{
    public float SlowDuration = 1f;
    public float SlowMult = 2f;
    //public LocalizedString slowText;

    [SerializeField] private ModifiableActionDescriptor EnhancementUpgradeResult;
    public ModifiableActionDescriptor upgradeResult { get => EnhancementUpgradeResult; set => EnhancementUpgradeResult = value; }
    public SlowingSlam1() { }

    public SlowingSlam1(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour,float SlowAmount, float SlowDuration, bool DoesPrevent, ModifiableActionDescriptor result) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.SlowDuration = SlowDuration;
        SlowMult = SlowAmount;
        upgradeResult = result;
    }
    protected override void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

        //slamImpactField = slamVariablesAccess.SlamImpactField;
        // Debug.Log("SLAM STRTED");

        slamOrigin = ownerEntity.target.transform.position;

        // + ownerEntity.transform.TransformPoint(slamVariablesAccess.slamPositionOffset);
        //EnemyAttackImpactField field = slamVariablesAccess.SPAWNTHING(slamVariablesAccess.DebugSlamObj, slamOrigin).GetComponent<EnemyAttackImpactField>();
        SpawnSlamStartVFX();
    }
    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(SlowMult, "Slow"),
                new List<BaseCondition> { new TimeCondition(true, SlowDuration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new SlowingSlam1(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, SlowMult, SlowDuration, preventsMovement, upgradeResult);
    }
}
