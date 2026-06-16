using System;
using UnityEngine;

[Serializable]
public class DebugAction : BaseEntityAction
{
    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        isComplete = true;
        Debug.Log("Debug Action Started");
    }
    public override void UpdateAction()
    {
    }
    public override void FixedUpdateAction()
    {
    }
    public override void InterruptAction()
    {
    }
    public override void EndAction()
    {
    }
    public override BaseEntityAction Clone()
    {
        return new DebugAction();
    }
}
