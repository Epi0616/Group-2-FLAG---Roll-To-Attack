using System;
using UnityEngine;

public interface ICondition
{
    public void ConditionUpdate();

    public void ResetCondition();

    public bool IsConditionMet();
}

[Serializable]
public abstract class BaseCondition : ICondition
{
    public bool isRequired = false;

    public string name = "Base";

    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsConditionMet();
}

[Serializable]
public class HealthCondition : BaseCondition
{
    public int healthAmount;

    public HealthCondition(int currentHealth)
    {
        healthAmount = currentHealth;
    }

    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return false;
    }
}
