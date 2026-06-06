using System;
using UnityEngine;

[Serializable]
public class BaseEntityAction : IAction
{
    public bool isComplete { get; set; }
    public bool preventsMovement { get; set; }
    protected Entity ownerEntity;
    protected IActionable actionable;
    public virtual void StartAction(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        actionable = ownerEntity as IActionable;
        isComplete = false;
    }
    public virtual void UpdateAction()
    {
    }
    public virtual void FixedUpdateAction()
    { 
    }
    public virtual void InterruptAction()
    {
    }
    public virtual void EndAction()
    {
    }
}
