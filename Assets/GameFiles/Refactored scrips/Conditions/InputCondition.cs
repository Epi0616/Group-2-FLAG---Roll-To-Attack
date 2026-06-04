using System;
using UnityEngine;

[Serializable]
public class InputCondition : BaseCondition
{
    private Entity ownerEntity;

    public InputCondition() { }
    public InputCondition(Entity entity)
    {
        ownerEntity = entity;
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        bool conditionMet = true;
        if (ownerEntity is not IUsesEntityInput) { conditionMet = false; }
        Debug.Log(ownerEntity);
        Debug.Log("input condition: " + conditionMet);
        return conditionMet;
    }
}
