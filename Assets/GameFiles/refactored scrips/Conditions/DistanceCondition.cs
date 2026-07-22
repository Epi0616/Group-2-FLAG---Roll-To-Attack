using System;
using UnityEngine;

[Serializable]
public class DistanceCondition : BaseCondition
{
    private Entity entity;
    public float distanceThreshold;

    public DistanceCondition() { }

    public DistanceCondition(bool inverse, float distanceThreshold)
    {
        this.inverse = inverse;
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
        if (inverse) return !((entity.target.transform.position - entity.transform.position).magnitude < distanceThreshold);
        return (entity.target.transform.position - entity.transform.position).magnitude < distanceThreshold;
    }
    public override BaseCondition Clone()
    {
        return new DistanceCondition(inverse, distanceThreshold);
    }
}
