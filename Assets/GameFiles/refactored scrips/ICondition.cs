using System;
using UnityEngine;

public interface ICondition
{
    public void ConditionUpdate();

    public void ResetCondition();

    public bool IsConditionMet();
}

[Serializable]
public abstract class BaseCondition : ICondition
{
    public bool isRequired = false;

    public string name = "Base";

    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsConditionMet();
}

[Serializable]
public class HealthCondition : BaseCondition
{
    public int healthAmount;

    public HealthCondition() { }

    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return false;
    }
}

[Serializable]
public class DistanceCondition: BaseCondition
{
    private Transform targetPos;
    private Entity entity;
    public float distanceThreshold;

    public DistanceCondition() { }
    
    public DistanceCondition(bool isRequired, Entity entity, Transform target, float distanceThreshold)
    {
        this.isRequired = isRequired;
        targetPos = target;
        this.entity = entity;
        this.distanceThreshold = distanceThreshold;
    }

    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return (targetPos.position - entity.transform.position).magnitude < distanceThreshold;
    }
}
