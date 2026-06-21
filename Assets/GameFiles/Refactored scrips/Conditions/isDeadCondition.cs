using UnityEngine;
using System;

[System.Serializable]
public class isDeadCondition : BaseCondition
{
    private EntityHealthSystem EHS;

    public isDeadCondition() { }
    public isDeadCondition(bool inverse)
    {
        this.inverse = inverse;
    }
    public override void Initialize(Entity entity)
    {
        EHS = entity.healthSystem;
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        if (inverse) return !EHS.isDead;
        return EHS.isDead;
    }

    public override BaseCondition Clone()
    {
        return new isDeadCondition(inverse);
    }
}
