using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ApplyShield : BaseEntityAction
{
    [SerializeField] private int shieldStacks;

    public ApplyShield() { }
    public ApplyShield(int shieldStacks)
    {
        this.shieldStacks = shieldStacks;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        ActiveStatusEffect shieldEffect = new(new ShieldedStatus(shieldStacks),  new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);
        EndAction();
    }
    public override void UpdateAction()
    {
    }
    public override void FixedUpdateAction()
    {
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
        return new ApplyShield(shieldStacks);
    }
}
