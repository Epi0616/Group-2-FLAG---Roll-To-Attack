using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShieldDestroyed : BaseEntityAction
{
    private IShieldable shieldable;

    public ShieldDestroyed() { }
    public ShieldDestroyed(bool preventsMovement)
    {
        this.preventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        shieldable = ownerEntity as IShieldable;
        Debug.Log(preventsMovement);
    }
    public override void UpdateAction()
    {
        if (shieldable.shielded)
        {
            EndAction();
        }
    }
    public override void InterruptAction()
    {
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new ShieldDestroyed(preventsMovement);
    }
}
