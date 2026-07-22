using System;
using UnityEngine;

[Serializable]
public class ShieldsDown : BaseCondition
{
    private IShieldable shieldable;

    public ShieldsDown() { }

    public override void Initialize(Entity entity)
    {
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
        return !shieldable.shielded;
    }
    public override BaseCondition Clone()
    {
        return new ShieldsDown();
    }
}
