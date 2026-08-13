using System;
using UnityEngine;

[Serializable]
public class ExpandingRing : BaseEntityAction
{
    public ExpandingRing() { }

    public override void StartAction(Entity ownerEntity)
    {
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new ExpandingRing();
    }
}
