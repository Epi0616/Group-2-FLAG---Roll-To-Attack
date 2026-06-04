using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InputMovementState : IEntityBaseMovementState
{
    Entity ownerEntity;
    private IMoveable moveable;
    private IUsesEntityInput usesEntityInput;
    bool activeState = false;
    public InputMovementState(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }

    public virtual void EnterState()
    {
        moveable = ownerEntity as IMoveable;
        usesEntityInput = ownerEntity as IUsesEntityInput;

        if (moveable == null) return;
        if (usesEntityInput == null) return;

        activeState = true;
    }
    public virtual void UpdateState()
    {
        if (!activeState) return;

        Vector3 direction = usesEntityInput.inputManager.moveDirection;
        float movementSpeed = moveable.movementSpeed.GetFinalValue();

        ownerEntity.transform.position += direction * movementSpeed * Time.deltaTime;
    }
    public virtual void FixedUpdateState()
    {

    }
    public virtual void ExitState()
    {
        activeState = false;
    }
}
