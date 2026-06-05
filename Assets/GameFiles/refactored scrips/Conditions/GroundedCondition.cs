using UnityEngine;
using System;

[Serializable]
public class GroundedCondition : BaseCondition
{
    private Entity ownerEntity;
    private IGrounded groundInterfaceAccess;

    GroundedCondition() { }
    GroundedCondition(bool required, Entity entity)
    {
        isRequired = required;
        ownerEntity = entity;
    }

    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        groundInterfaceAccess = ownerEntity as IGrounded;
    }
    public override void ConditionUpdate() { }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {
        if (groundInterfaceAccess == null) {  return false; }
        return groundInterfaceAccess.isGrounded;
    }
}
