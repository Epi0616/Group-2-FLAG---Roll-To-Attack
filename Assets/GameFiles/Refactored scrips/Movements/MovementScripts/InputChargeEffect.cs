using System;
using UnityEngine;

[Serializable]
public class InputChargeEffect : BaseEntityMovement
{
    private IUsesEntityInput usesEntityInput;
    private PlayerBodySystem playerBodySystem;
    private bool chargeComplete;
    public InputChargeEffect() { }
    public override void StartMovement(Entity ownerEntity)
    {
        chargeComplete = false;
        base.StartMovement(ownerEntity);
        playerBodySystem = ownerEntity.bodySystem as PlayerBodySystem;        
        usesEntityInput = ownerEntity as IUsesEntityInput;
        Debug.Log("start wobble");
    }
    public override void UpdateMovement()
    {
        float holdTime = usesEntityInput.inputManager.holdTime;
        float moveSpeedMultiplier = ((2 - holdTime) / 2);
        moveSpeedMultiplier = Mathf.Clamp(moveSpeedMultiplier, 0.35f, 1);

        ownerEntity.bodySystem.Wobble(2 / moveSpeedMultiplier);
        ChargeParticles(holdTime);
    }
    public override void InterruptMovement()
    {
        ownerEntity.bodySystem.body.transform.rotation = ownerEntity.bodySystem.originalRotation;
        playerBodySystem.ResetChargingEffects();
    }
    public override void EndMovement()
    {
        ownerEntity.bodySystem.body.transform.rotation = ownerEntity.bodySystem.originalRotation;
        playerBodySystem.ResetChargingEffects();
    }
    public override BaseEntityMovement Clone()
    {
        return new InputChargeEffect();
    }

    private void ChargeParticles(float holdTime)
    {
        if (holdTime < 1)
        {
            playerBodySystem.DisplayChargingEffect();
            return;
        }
        if (!chargeComplete)
        {
            playerBodySystem.ResetChargingEffects();
            playerBodySystem.DisplayChargeCompleteEffect();
            chargeComplete = true;
        }
    }
}
