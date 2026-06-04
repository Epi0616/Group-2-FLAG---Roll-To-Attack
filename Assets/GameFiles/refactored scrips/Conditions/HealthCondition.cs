using System;
using UnityEngine;

[Serializable]
public class HealthCondition : BaseCondition
{
    public int healthAmount;

    public HealthCondition() { }
    public override void Initialize(Entity entity) { }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return false;
    }
}
