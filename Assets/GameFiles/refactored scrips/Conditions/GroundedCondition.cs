using UnityEngine;
using System;

[Serializable]
public class GroundedCondition : BaseCondition
{
    private Entity ownerEntity;
    private IGrounded groundInterfaceAccess;

    public GroundedCondition() { }
    public GroundedCondition(bool inverse)
    {
        this.inverse = inverse;
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
        if (groundInterfaceAccess == null) {  return true; }
        return groundInterfaceAccess.isGrounded;
    }

    public override BaseCondition Clone()
    {
        return new GroundedCondition(inverse);
    }
}
