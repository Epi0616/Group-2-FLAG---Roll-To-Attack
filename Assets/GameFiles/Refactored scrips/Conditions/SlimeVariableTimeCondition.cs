using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SlimeVariableTimeCondition : VariableTimeCondition
{
    public SlimeVariableTimeCondition() { }

    public SlimeVariableTimeCondition(bool inverse, float duration, float variance)
    { 
        this.inverse = inverse;
        this.duration = duration;
        this.variance = variance;
    }

    private ISlimeSplit slimeSplit;
    public override void Initialize(Entity entity)
    {
        if (!(entity is ISlimeSplit slimeSplit)) { Debug.LogError("entity is not of type ISlimeSplit"); return; }
        this.slimeSplit = slimeSplit;

        base.Initialize(entity);
    }

    protected override void SetTimer()
    {
        float variation = Random.Range(-variance, variance);
        timer = (duration + variation) * MathF.Pow(slimeSplit.scale,2);
    }

    public override BaseCondition Clone()
    {
        return new SlimeVariableTimeCondition(inverse, duration, variance);
    }
}
