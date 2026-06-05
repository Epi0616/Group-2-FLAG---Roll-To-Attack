using UnityEngine;
using System;

[Serializable]
public class TimeCondition : BaseCondition
{
    private float timer;
    public float duration;
    private float totalTimeElapsed;

    private Entity ownerEntity;

    public TimeCondition() { }

    public TimeCondition(bool required, Entity entity, float duration)
    {
        ownerEntity = entity;
        isRequired = required;
        this.duration = duration;
        timer = duration;
        name = "DurationCondition";
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
    }

    public override void ConditionUpdate()
    {
        timer -= Time.deltaTime;
        totalTimeElapsed += Time.deltaTime;
    }

    public override void ResetCondition()
    {
        timer = duration;
    }

    public override bool IsConditionMet()
    {
        return timer <= 0;
    }
}
