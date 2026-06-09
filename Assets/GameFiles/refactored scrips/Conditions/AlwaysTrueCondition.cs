using UnityEngine;
using System;

[Serializable]
public class AlwaysTrueCondition : BaseCondition
{
    public AlwaysTrueCondition() { }
    public AlwaysTrueCondition(bool inverse)
    {
        this.inverse = inverse;
    }
    public override void Initialize(Entity entity) { }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        if (inverse) return false;
        return true;
    }
    public override BaseCondition Clone()
    {
        return new AlwaysTrueCondition(inverse);
    }
}
