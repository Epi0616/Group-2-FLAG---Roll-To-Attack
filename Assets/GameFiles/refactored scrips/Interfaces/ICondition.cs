using UnityEngine;

public interface ICondition
{
    public void Initialize(Entity entity);
    public void ConditionUpdate();

    public void ResetCondition();

    public bool IsConditionMet();
}
