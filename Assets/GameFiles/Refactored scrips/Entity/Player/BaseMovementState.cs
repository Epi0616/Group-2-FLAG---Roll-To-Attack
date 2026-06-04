using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseMovementState : IEntityBaseMovementState
{
    Entity ownerEntity;
    bool activeState = false;
    public BaseMovementState(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }

    public virtual void EnterState()
    {
        if (!typeof(IMoveable).IsAssignableFrom(ownerEntity.GetType())) return;
        if (!typeof(IUsesEntityInput).IsAssignableFrom(ownerEntity.GetType())) return;

        activeState = true;
    }
    public virtual void UpdateState()
    {
        if (!activeState) return;

        Vector3 direction = (ownerEntity as IUsesEntityInput).inputManager.moveDirection;
        float movementSpeed = (ownerEntity as IMoveable).movementSpeed.GetFinalValue();

        ownerEntity.transform.position += direction * movementSpeed * Time.deltaTime;
    }
    public virtual void FixedUpdateState()
    {

    }
    public virtual void ExitState()
    {
        ownerEntity = null;
    }
}
