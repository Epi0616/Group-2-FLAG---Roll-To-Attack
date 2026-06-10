using System;
using UnityEngine;

[Serializable]
public class ChargeWobble : BaseEntityMovement
{
    private IUsesEntityInput usesEntityInput;
    public ChargeWobble() { }
    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        usesEntityInput = ownerEntity as IUsesEntityInput;
        Debug.Log("start wobble");
    }
    public override void UpdateMovement()
    {
        float moveSpeedMultiplier = ((2 - usesEntityInput.inputManager.holdTime) / 2);
        moveSpeedMultiplier = Mathf.Clamp(moveSpeedMultiplier, 0.35f, 1);

        ownerEntity.bodySystem.Wobble(2 / moveSpeedMultiplier);
    }
    public override void InterruptMovement()
    {
        ownerEntity.transform.rotation = ownerEntity.bodySystem.originalRotation;
    }
    public override void EndMovement()
    {
        ownerEntity.transform.rotation = ownerEntity.bodySystem.originalRotation;
    }
    public override BaseEntityMovement Clone()
    {
        return new ChargeWobble();
    }
}
