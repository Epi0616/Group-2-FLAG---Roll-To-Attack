using UnityEngine;
using System;

[Serializable]
public class InputChargeCondition : BaseCondition
{
    private IUsesEntityInput usesEntityInput;
    private IGrounded grounded;

    public InputChargeCondition() { }
    public override void Initialize(Entity entity) 
    {
        usesEntityInput = entity as IUsesEntityInput;
        grounded = entity as IGrounded;
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
        if (usesEntityInput.inputManager.holdTime <= 0.05f) { conditionMet = false; }
        if (!grounded.isGrounded) { conditionMet = false; }
        
        return conditionMet;
    }
    public override BaseCondition Clone()
    {
        return new InputChargeCondition();
    }
}
