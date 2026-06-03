using Unity.VisualScripting;
using UnityEngine;

public class BaseMovementState : IEntityBaseMovementState
{
    Player ownerEntity;
    public BaseMovementState(Player ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }

    public virtual void EnterState()
    {

    }
    public virtual void UpdateState()
    {
        Debug.Log("reached");
        ownerEntity.transform.position += ownerEntity.inputManager.moveDirection * ownerEntity.movementSpeed.GetFinalValue() * Time.deltaTime;
    }
    public virtual void FixedUpdateState()
    {

    }
    public virtual void ExitState()
    {
        ownerEntity = null;
    }
}
