using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BasicAttackCondition : BaseCondition
{
    private Entity ownerEntity;
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
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        bool conditionMet = true;
        if (ownerEntity is not IUsesEntityInput) { conditionMet = false; }
        if (!usesEntityInput.inputManager.attack.action.WasPressedThisFrame()) { conditionMet = false; }
        //if (!(ownerEntity as IGrounded).isGrounded) { conditionMet = false; }

        return conditionMet;
    }
}
