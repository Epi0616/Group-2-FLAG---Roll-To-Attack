using System;
using UnityEngine;

[Serializable]
public class SlimeIteration : BaseCondition
{
    [SerializeField] private int iterationsLeft;

    private ISlimeSplit slimeSplit;

    public SlimeIteration() { }
    public SlimeIteration(int iterationsLeft)
    { 
        this.iterationsLeft = iterationsLeft;
    }

    public override void Initialize(Entity entity)
    {
        if (!(entity is ISlimeSplit slimeSplit)) { Debug.LogError("entity is not of type ISlimeSplit"); return; }
        this.slimeSplit = slimeSplit;
    }

    public override void ConditionUpdate()
    {

    }

    public override void ResetCondition()
    {

    }

    public override bool IsConditionMet()
    {
        return slimeSplit.iterationsLeft == iterationsLeft;
    }
    public override BaseCondition Clone()
    {
        return new SlimeIteration(iterationsLeft);
    }
}
