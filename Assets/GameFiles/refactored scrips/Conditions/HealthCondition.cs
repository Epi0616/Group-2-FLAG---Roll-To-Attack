using System;
using UnityEngine;

[Serializable]
public class HealthCondition : BaseCondition
{
    [Header("percentage 1-100")]
    [SerializeField] protected float healthThresholdPercentage;
    protected Entity ownerEntity;

    public HealthCondition() { }
    public HealthCondition(bool inverse, float healthThresholdPercentage)
    {
        this.inverse = inverse;
        this.healthThresholdPercentage = healthThresholdPercentage;
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
        //Debug.Log("current health percent: " + ownerEntity.healthSystem.currentHealth / ownerEntity.healthSystem.maxHealth);
        if ((float)ownerEntity.healthSystem.currentHealth / (float)ownerEntity.healthSystem.maxHealth.GetFinalValue() <= healthThresholdPercentage / 100)
        {
            return !inverse;
        }
        return inverse;
    }
    public override BaseCondition Clone()
    {
        return new HealthCondition(inverse, healthThresholdPercentage);
    }
}
