using UnityEngine;
using System;

[Serializable]
public class FacingTargetCondition : BaseCondition
{
    private Entity entity;
    public float lookThreshold;

    public FacingTargetCondition() { }

    public FacingTargetCondition(bool inverse, float lookThreshold)
    {
        this.inverse = inverse;

        this.lookThreshold = lookThreshold;
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
        Vector3 dir = entity.target.transform.position - entity.transform.position;
        dir.y = 0f;

        float result = Vector3.Angle(entity.transform.forward, dir);


        if (inverse) return result > lookThreshold;
        return result < lookThreshold;
    }
    public override BaseCondition Clone()
    {
        return new FacingTargetCondition(inverse, lookThreshold);
    }
}
