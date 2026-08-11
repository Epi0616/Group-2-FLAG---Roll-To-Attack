using UnityEngine;
using System;

[Serializable]
public class TimeCondition : BaseCondition
{
    private float timer;
    public float duration;
    private float totalTimeElapsed;

    public TimeCondition() { }

    public TimeCondition(bool inverse, float duration)
    {
        this.inverse = inverse;
        this.duration = duration;
        timer = duration;
    }
    public override void Initialize(Entity entity)
    {
    }

    public override void ConditionUpdate()
    {
        timer -= Time.deltaTime;
        totalTimeElapsed += Time.deltaTime;

        //Debug.Log($"counting down: {timer}");
    }

    public override void ResetCondition()
    {
        timer = duration;
        Debug.Log("resetting condition");
    }

    public override bool IsConditionMet()
    {
        return timer <= 0;
    }
    public override BaseCondition Clone()
    {
        return new TimeCondition(inverse, duration);
    }
}
