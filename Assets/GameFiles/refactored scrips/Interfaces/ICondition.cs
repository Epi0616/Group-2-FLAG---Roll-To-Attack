using UnityEngine;

public interface ICondition
{
    public void ConditionUpdate();

    public void ResetCondition();

    public bool IsConditionMet();
}
