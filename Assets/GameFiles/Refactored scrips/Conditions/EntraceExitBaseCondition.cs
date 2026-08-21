using UnityEngine;

public class EntraceExitBaseCondition : BaseCondition
{
    protected Entity ownerEntity;
    protected bool entered = false;
    protected bool isConditionMet = false;

    public override void Initialize(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }

    public override void ConditionUpdate()
    {
        if (!entered)
        { 
            isConditionMet = CheckEntranceCondition();
            return;
        }

        isConditionMet = CheckExitConditon();
    }

    public override void ResetCondition()
    {

    }

    public override bool IsConditionMet()
    {
        return inverse ? !isConditionMet : isConditionMet;
    }

    protected virtual bool CheckEntranceCondition()
    { 
        entered = true;
        return true;
    }

    protected virtual bool CheckExitConditon()
    {
        entered = false;
        return true;
    }

    public override BaseCondition Clone()
    {
        return new EntraceExitBaseCondition();
    }
}
