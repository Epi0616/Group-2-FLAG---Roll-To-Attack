using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SlamBlockedCondition : BaseCondition
{
    //Based on ShieldCondition

    private bool activatingBlock = false;
    private ISlamBlock slamBlock;
    private Entity ownerEntity;

    public SlamBlockedCondition() { }

    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        slamBlock = entity as ISlamBlock;
    }
    public override void ConditionUpdate()
    {
    }

    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return !slamBlock.blockingSlam;
    }
    public override BaseCondition Clone()
    {
        return new SlamBlockedCondition();
    }
}

