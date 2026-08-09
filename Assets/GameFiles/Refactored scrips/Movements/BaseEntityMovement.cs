using System;
using UnityEngine;

[Serializable]
public class BaseEntityMovement : IMovement
{
    protected Entity ownerEntity;
    protected IMoveable moveable;

    public BaseEntityMovement() {}
    public virtual void StartMovement(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        moveable = ownerEntity as IMoveable;
    }
    public virtual void UpdateMovement()
    {
    }
    public virtual void FixedUpdateMovement()
    { 
    }
    public virtual void InterruptMovement()
    {
    }
    public virtual void EndMovement()
    {
    }
    public virtual BaseEntityMovement Clone()
    { 
        return new BaseEntityMovement();
    }
}
