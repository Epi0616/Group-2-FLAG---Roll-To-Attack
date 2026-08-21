using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class FacingPoint : FacingTargetCondition
{
    [SerializeField] protected Vector3 point;

    public FacingPoint() { }
    public FacingPoint(bool inverse, Vector3 point, float lookThreshold)
    {
        this.inverse = inverse;
        this.point = point;
        this.lookThreshold = lookThreshold;
    }

    public override bool IsConditionMet()
    {
        Vector3 dir = point - ownerEntity.transform.position;
        dir.y = 0f;

        float result = Vector3.Angle(ownerEntity.transform.forward, dir);

        bool isConditionMet = result < lookThreshold;
        return inverse ? !isConditionMet : isConditionMet;
    }

    public override BaseCondition Clone()
    {
        return new FacingPoint(inverse, point, lookThreshold);
    }
}
