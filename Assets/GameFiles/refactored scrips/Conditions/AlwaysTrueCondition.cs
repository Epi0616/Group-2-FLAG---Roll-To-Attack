using UnityEngine;
using System;

[Serializable]
public class AlwaysTrueCondition : BaseCondition
{
    public AlwaysTrueCondition() { }
    public override void Initialize(Entity entity) { }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return true;
    }
}
