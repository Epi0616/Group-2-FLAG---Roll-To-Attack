using System;
using UnityEngine;

[Serializable]
public abstract class BaseCondition : ICondition
{
    public bool isRequired = false;

    public string name = "Base";

    public abstract void ConditionUpdate();

    public abstract void ResetCondition();

    public abstract bool IsConditionMet();
}
