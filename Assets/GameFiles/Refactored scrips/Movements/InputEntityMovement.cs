using System;
using UnityEngine;

[Serializable]
public class InputEntityMovement : BaseEntityMovement
{
    private IUsesEntityInput usesEntityInput;
    public InputEntityMovement() { }
    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        usesEntityInput = ownerEntity as IUsesEntityInput;
    }
    public override void UpdateMovement()
    {
        Vector3 direction = usesEntityInput.inputManager.move.action.ReadValue<Vector3>();
        float movementSpeed = moveable.movementSpeed.GetFinalValue();

        ownerEntity.transform.position += direction * movementSpeed * Time.deltaTime;
    }
    public override void InterruptMovement()
    {
    }
    public override void EndMovement()
    {
    }
    public override BaseEntityMovement Clone()
    {
        return new InputEntityMovement();
    }
}
