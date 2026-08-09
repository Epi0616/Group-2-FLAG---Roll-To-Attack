using System;
using UnityEngine;

[Serializable]
public class DistanceFromPointAndFacing : BaseCondition
{
    private Entity entity;
    public float distanceThreshold;
    [SerializeField] private Vector3 point;

    public DistanceFromPointAndFacing() { }

    public DistanceFromPointAndFacing(bool inverse, float distanceThreshold, Vector3 point)
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
        Vector3 dir = point - entity.transform.position;
        dir.y = 0f;
        float result = Vector3.Angle(entity.transform.forward, dir);

        bool isConditionMet = ((point - entity.transform.position).magnitude < distanceThreshold) || result > 90f;

        return inverse ? !isConditionMet : isConditionMet;
    }
    public override BaseCondition Clone()
    {
        return new DistanceFromPointAndFacing(inverse, distanceThreshold, point);
    }
}
