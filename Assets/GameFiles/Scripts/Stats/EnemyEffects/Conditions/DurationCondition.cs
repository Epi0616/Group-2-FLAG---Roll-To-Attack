using UnityEngine;

public class DurationCondition : BaseCondition
{
    private float timer; 
    private float duration;
    private float totalDuration;

    public DurationCondition(bool required, float duration)
    {
        isRequired = required;
        this.duration = duration;
        timer = duration;
        name = "DurationCondition";
    }
    public override void Initialize(Entity entity)
    {
        //this.entity = entity;
    }
    public override void ConditionUpdate()
    {
        timer -= Time.deltaTime;
        totalDuration += Time.deltaTime;
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
