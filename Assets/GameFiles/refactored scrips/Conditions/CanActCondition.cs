using UnityEngine;
using System;

[Serializable]
public class CanActCondition : BaseCondition
{
    private IActionable act;
    private bool valid = true;

    public CanActCondition() { }
    public CanActCondition(bool inverse)
    {
        this.inverse = inverse;
    }
    public override void Initialize(Entity entity)
    {
        act = entity as IActionable;
        valid = true;
        if (act == null) { valid = false; }
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        if (valid)
        {
            return act.canAct;
        }
        return false;
    }

    public override BaseCondition Clone()
    {
        return new CanActCondition(inverse);
    }
}
