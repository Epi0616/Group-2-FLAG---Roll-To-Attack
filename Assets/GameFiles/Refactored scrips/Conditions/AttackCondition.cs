using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AttackCondition : BaseCondition
{
    private Entity ownerEntity;
    private IUsesEntityInput usesEntityInput;
    private IJumpable jumpable;
    private bool conditionMet = false;

    public AttackCondition() { }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        usesEntityInput = entity as IUsesEntityInput;
        jumpable = entity as IJumpable;
        conditionMet = false;
    }
    public override void ConditionUpdate()
    {
        if (usesEntityInput == null) return;

        float holdTime = usesEntityInput.inputManager.holdTime;
        if (usesEntityInput.inputManager.attack.action.WasPressedThisFrame())
        { 
            conditionMet = true; 
            Debug.Log("basic attack");
        }
        else if (usesEntityInput.inputManager.attack.action.WasReleasedThisFrame() && holdTime >= 0.2f)
        {
            conditionMet = true;
            jumpable.jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            jumpable.impactSpeed.AddMultiplierFlat(holdTime * 2);
            Debug.Log("charged attack");
        }
    }
    public override void ResetCondition()
    {
        conditionMet = false;
    }
    public override bool IsConditionMet()
    {
        if (ownerEntity is not IUsesEntityInput) { conditionMet = false; }
        //if (!(ownerEntity as IGrounded).isGrounded) { conditionMet = false; }

        return conditionMet;
    }
    public override BaseCondition Clone()
    {
        return new AttackCondition();
    }
}
