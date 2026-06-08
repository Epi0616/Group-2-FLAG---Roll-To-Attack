using System;
using UnityEngine;

[Serializable]
public class DistanceCondition : BaseCondition
{
    private Transform targetPos;
    private Entity entity;
    public float distanceThreshold;

    public DistanceCondition() { }

    public DistanceCondition(bool isRequired, Transform target, float distanceThreshold)
    {
        this.isRequired = isRequired;
        targetPos = target;
        this.distanceThreshold = distanceThreshold;
    }
    public override void Initialize(Entity entity) 
    {
        this.entity = entity;
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return (entity.target.transform.position - entity.transform.position).magnitude < distanceThreshold;
    }
    public override BaseCondition Clone()
    {
        return new DistanceCondition(isRequired, targetPos, distanceThreshold);
    }
}
