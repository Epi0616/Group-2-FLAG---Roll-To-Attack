using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class InputMovementCondition : BaseCondition
{
    private Entity ownerEntity;

    public InputMovementCondition() { }
    public InputMovementCondition(Entity entity) 
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
        if (ownerEntity is not IMoveable) { conditionMet = false; }
        if (ownerEntity is not IUsesEntityInput) { conditionMet = false; }

        return conditionMet;
    }
    public override BaseCondition Clone()
    {
        return new InputMovementCondition();
    }
}
