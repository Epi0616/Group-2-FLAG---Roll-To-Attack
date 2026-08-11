using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class VariableTimeCondition : BaseCondition
{
    protected float timer;
    public float duration;
    public float variance;
    protected float totalTimeElapsed;

    public VariableTimeCondition() { }

    public VariableTimeCondition(bool inverse, float duration, float variance)
    {
        this.inverse = inverse;
        this.duration = duration;
        this.variance = variance;
    }
    public override void Initialize(Entity entity)
    {
        SetTimer();
    }

    public override void ConditionUpdate()
    {
        timer -= Time.deltaTime;
        totalTimeElapsed += Time.deltaTime;
    }
    
    public override void ResetCondition()
    {
        SetTimer();
    }

    protected virtual void SetTimer()
    { 
        float variation = Random.Range(-variance, variance);
        timer = duration + variation;
    }

    public override bool IsConditionMet()
    {
        return timer <= 0;
    }
    public override BaseCondition Clone()
    {
        return new VariableTimeCondition(inverse, duration, variance);
    }
}
