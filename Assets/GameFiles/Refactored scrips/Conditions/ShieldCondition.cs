using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ShieldCondition : BaseCondition
{
    private bool activatingShield = false;
    private IShieldable shieldable;
    private Entity ownerEntity;

    public ShieldCondition() { }

    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        shieldable = entity as IShieldable;
    }
    public override void ConditionUpdate()
    {
    }

    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return !(shieldable.currentShieldStacks > 0);
    }
    public override BaseCondition Clone()
    {
        return new ShieldCondition();
    }
}
