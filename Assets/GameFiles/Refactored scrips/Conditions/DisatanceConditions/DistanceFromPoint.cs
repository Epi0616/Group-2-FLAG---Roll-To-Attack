using System;
using UnityEngine;

[Serializable]
public class DistanceFromPoint : BaseCondition
{
    private Entity entity;
    public float distanceThreshold;
    [SerializeField] private Vector3 point;

    public DistanceFromPoint() { }

    public DistanceFromPoint(bool inverse, float distanceThreshold, Vector3 point)
    {
        this.inverse = inverse;
        this.distanceThreshold = distanceThreshold;
        this.point = point;
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
        bool isConditionMet = (point - entity.transform.position).magnitude < distanceThreshold;    

        return inverse ? !isConditionMet : isConditionMet;
    }
    public override BaseCondition Clone()
    {
        return new DistanceFromPoint(inverse, distanceThreshold, point);
    }
}
