using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalAction
{
    [SerializeReference, SubclassSelector]
    public IAction action;
    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public bool allConditionsRequired = false;
    public bool singleUse = false, triggered = false;
    public bool exclusive = true;
    public int priority = 0;

    public ConditionalAction() { }

    public ConditionalAction(IAction action, List<BaseCondition> conditions, bool singleUse, bool exclusive, int priority, bool allConditionsRequired)
    {
        this.action = action;
        this.conditions = conditions;
        this.singleUse = singleUse;
        this.exclusive = exclusive;
        this.priority = priority;
        this.allConditionsRequired = allConditionsRequired;
    }

    public void UpdateConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ConditionUpdate();
        }
    }

    public void ResetConditionsAll()
    {
        foreach (BaseCondition condition in conditions)
        {
            condition.ResetCondition();
        }
    }

}
