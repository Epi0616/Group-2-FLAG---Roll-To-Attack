using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ApplyShieldLevel2 : BaseEntityAction
{
    [SerializeField] private int shieldStacks;
    public ApplyShieldLevel2() { }
    public ApplyShieldLevel2(int shieldStacks)
    {
        //1. somthing is missing here
        this.shieldStacks = shieldStacks;
        // defining and assigning the shield stacks integer to the value in the constructor
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        //1. how can you edit this script to apply a custom amount of shield stacks?
        ActiveStatusEffect shieldEffect = new(new ShieldedStatus(shieldStacks),  new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);
        // replacing the 5 with the shield stacks variable to apply a custom amount of shield stacks
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
        //2. somthing is missing from this function, what needs to happen for an action to end?
        isComplete = true;

    }
    public override BaseEntityAction Clone()
    {
        return new ApplyShield(shieldStacks);
    }
}
