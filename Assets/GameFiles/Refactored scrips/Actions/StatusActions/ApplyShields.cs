using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplyShields : BaseEntityAction
{
    [SerializeField] private int shieldStacks;

    public ApplyShields() { }
    public ApplyShields(int shieldStacks)
    { 
        this.shieldStacks = shieldStacks;
    }

    public override void StartAction(Entity ownerEntity)
    {
        ActiveStatusEffect shieldEffect = new(new ShieldedStatus(shieldStacks), new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);

        EndAction();
    }

    public override void EndAction()
    {
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new ApplyShields(shieldStacks);
    }
}
