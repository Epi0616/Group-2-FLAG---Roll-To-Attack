using System;
using UnityEngine;

[Serializable]
public class HealthIntervalCondition : BaseCondition
{
    [Header("percentage 0-1")]
    [SerializeField] protected float healthIntervalPercentage;
    protected Entity ownerEntity;

    private float currentHealthGate;

    public HealthIntervalCondition() { }
    public HealthIntervalCondition(bool inverse, float healthIntervalPercentage)
    {
        this.inverse = inverse;
        this.healthIntervalPercentage = healthIntervalPercentage;
    }
    public override void Initialize(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
        currentHealthGate = GetCurrentHealthGate();
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
        currentHealthGate = GetCurrentHealthGate();
    }
    public override bool IsConditionMet()
    {
        float currentHealthPercentage = GetCurrentHealthPercentage();

        bool isConditionMet = currentHealthPercentage <= currentHealthGate;
        return inverse ? !isConditionMet : isConditionMet;
    }

    private float GetCurrentHealthPercentage()
    { 
        return (float)ownerEntity.healthSystem.currentHealth / (float)ownerEntity.healthSystem.maxHealth.GetFinalValue();
    }

    private float GetCurrentHealthGate()
    {
        int intervals = Mathf.CeilToInt(GetCurrentHealthPercentage() / healthIntervalPercentage) - 1;
        float currentHealthGate = intervals * healthIntervalPercentage;

        return currentHealthGate;
    }

    public override BaseCondition Clone()
    {
        return new HealthIntervalCondition(inverse, healthIntervalPercentage);
    }
}