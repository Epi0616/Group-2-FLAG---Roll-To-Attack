using System;
using UnityEngine;

[Serializable]
public abstract class BaseCondition : ICondition
{
    public bool inverse = false;

    public abstract void Initialize(Entity ownerEntity);
    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsConditionMet();

    public abstract BaseCondition Clone();
}
