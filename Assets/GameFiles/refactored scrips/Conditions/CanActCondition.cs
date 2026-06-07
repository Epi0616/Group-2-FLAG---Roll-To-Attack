using UnityEngine;

public class CanActCondition : BaseCondition
{
    private IActionable act;
    private bool valid = true;

    public CanActCondition() { }
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
}
