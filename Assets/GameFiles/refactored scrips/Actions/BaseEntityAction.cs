using System;
using UnityEngine;

[Serializable]
public class BaseEntityAction : IAction
{
    [SerializeField] protected bool PreventsMovement;
    public bool isComplete { get; set; }
    public bool preventsMovement { get => PreventsMovement; set => PreventsMovement = value; }
    protected Entity ownerEntity;
    protected IActionable actionable;
    public virtual void StartAction(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        actionable = ownerEntity as IActionable;
        //isComplete = false;
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
        //isComplete = true; this really should be added in but i fear the consiquences
    }
    public virtual BaseEntityAction Clone()
    { 
        return new BaseEntityAction();
    }
}
