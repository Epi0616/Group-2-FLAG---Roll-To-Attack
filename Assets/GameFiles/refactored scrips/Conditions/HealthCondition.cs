using System;
using UnityEngine;

[Serializable]
public class HealthCondition : BaseCondition
{
    public int healthThreshold;
    public Entity ownerEntity;

    public HealthCondition() { }
    public HealthCondition(bool inverse, int healththreshold)
    {
        this.inverse = inverse;
        this.healthThreshold = healththreshold;
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        //if (inverse) return ownerEntity.healthSystem.currentHealth > healthThreshold;
        //return ownerEntity.healthSystem.currentHealth < healthThreshold;
        return false;
    }
    public override BaseCondition Clone()
    {
        return new HealthCondition(inverse, healthThreshold);
    }
}
