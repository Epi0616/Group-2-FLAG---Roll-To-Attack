using UnityEngine;

public interface IEffectExpirationCondition
{

    public void ConditionUpdate();

    public void ResetCondition();

    public bool IsExpired();
}

public abstract class BaseCondition : IEffectExpirationCondition
{
    public bool isRequired = false;

    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsExpired();
}