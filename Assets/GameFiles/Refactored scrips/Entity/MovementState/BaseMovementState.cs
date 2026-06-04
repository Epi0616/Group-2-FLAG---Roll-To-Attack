using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseMovementState : IEntityBaseMovementState
{
    Entity ownerEntity;
    private IMoveable moveable;
    bool activeState = false;
    public BaseMovementState(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }

    public virtual void EnterState()
    {
        moveable = ownerEntity as IMoveable;
        if (moveable == null) return;
    }
    public virtual void UpdateState()
    {
        if (!activeState) return;
    }
    public virtual void FixedUpdateState()
    {

    }
    public virtual void ExitState()
    {
        activeState = false;
    }
}
