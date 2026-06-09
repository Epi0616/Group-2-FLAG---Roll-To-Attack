using System;
using UnityEngine;

[Serializable]
public abstract class BaseCondition : ICondition
{
    public bool inverse = false;

    public string name = "Base";

    public abstract void Initialize(Entity entity);
    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsConditionMet();

    public abstract BaseCondition Clone();
}
