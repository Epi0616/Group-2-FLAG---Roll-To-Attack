using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfusedSlam : BaseSlamAction
{
    public float Duration = 1f;
    //public LocalizedString slowText;

    public ConfusedSlam() { }

    public ConfusedSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float Duration, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.Duration = Duration;
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
        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new ConfusedStatus(Duration, "PlaceHolderSlow"),
                new List<BaseCondition> { new TimeCondition(true, Duration) }, true), slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new ConfusedSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, Duration, true);
    }
}
