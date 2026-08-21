using System;
using UnityEngine;

[Serializable]
public class DistanceCondition : BaseCondition
{
    protected Entity ownerEntity;
    public float distanceThreshold;

    public DistanceCondition() { }

    public DistanceCondition(bool inverse, float distanceThreshold)
    {
        this.inverse = inverse;
        this.distanceThreshold = distanceThreshold;
    }
    public override void Initialize(Entity entity) 
    {
        this.ownerEntity = entity;
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        if (inverse) return !((ownerEntity.target.transform.position - ownerEntity.transform.position).magnitude < distanceThreshold);
        return (ownerEntity.target.transform.position - ownerEntity.transform.position).magnitude < distanceThreshold;
    }
    public override BaseCondition Clone()
    {
        return new DistanceCondition(inverse, distanceThreshold);
    }
}
