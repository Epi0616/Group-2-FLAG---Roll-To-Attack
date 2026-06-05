using System;
using UnityEngine;

[Serializable]
public class BasicAttackCondition : BaseCondition
{
    private Entity ownerEntity;
    private bool conditionMet;
    private IUsesEntityInput usesEntityInput;

    public BasicAttackCondition() { }
    public BasicAttackCondition(Entity entity)
    {
        ownerEntity = entity;
        usesEntityInput = entity as IUsesEntityInput;
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        usesEntityInput = entity as IUsesEntityInput;
    }
    public override void ConditionUpdate()
    {
        conditionMet = usesEntityInput.inputManager.attackWasPressedThisFrame;
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        //Debug.Log("attack condition: " + conditionMet);
        return conditionMet;
    }
}
